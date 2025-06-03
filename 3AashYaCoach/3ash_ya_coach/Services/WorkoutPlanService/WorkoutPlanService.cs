using _3AashYaCoach._3ash_ya_coach.Dtos;
using _3AashYaCoach._3ash_ya_coach.Models;
using _3AashYaCoach._3ash_ya_coach.Services.PlanSubscriptionService;
using _3AashYaCoach.Models.Context;
using _3AashYaCoach.Models.Enums;
using _3AashYaCoach.Models;
using Microsoft.EntityFrameworkCore;

namespace _3AashYaCoach._3ash_ya_coach.Services.WorkoutPlanService
{
    public class WorkoutPlanService : IWorkoutPlanService
    {
        private readonly AppDbContext _context;
        private readonly IPlanSubscriptionService _subscriptionService;

        public WorkoutPlanService(AppDbContext context, IPlanSubscriptionService subscriptionService)
        {
            _context = context;
            _subscriptionService = subscriptionService;
        }

        public async Task<string?> UpdatePlanWithDaysAndExercises(UpdateWorkoutPlanDto dto)
        {
            var plan = await _context.WorkoutPlans
                .Include(p => p.Days)
                    .ThenInclude(d => d.Exercises)
                .FirstOrDefaultAsync(p => p.Id == dto.PlanId);

            if (plan == null)
                return "Workout plan not found.";

            plan.PlanName = dto.PlanName;
            plan.PrimaryGoal = dto.PrimaryGoal;

            var updatedDayNumbers = dto.Days.Select(d => d.DayNumber).ToList();
            var daysToRemove = plan.Days.Where(d => !updatedDayNumbers.Contains(d.DayNumber)).ToList();
            _context.WorkoutDays.RemoveRange(daysToRemove);

            foreach (var dayDto in dto.Days)
            {
                var existingDay = plan.Days.FirstOrDefault(d => d.DayNumber == dayDto.DayNumber);

                if (existingDay != null)
                {
                    existingDay.DayName = dayDto.DayName;
                    existingDay.Notes = dayDto.Notes;

                    var updatedExerciseIds = dayDto.Exercises
                        .Where(e => e.Id.HasValue)
                        .Select(e => e.Id.Value).ToList();

                    var exercisesToRemove = existingDay.Exercises
                        .Where(e => !updatedExerciseIds.Contains(e.Id)).ToList();

                    _context.WorkoutExercises.RemoveRange(exercisesToRemove);

                    foreach (var exDto in dayDto.Exercises)
                    {
                        if (exDto.Id.HasValue)
                        {
                            var existingEx = existingDay.Exercises.FirstOrDefault(e => e.Id == exDto.Id.Value);
                            if (existingEx != null)
                            {
                                existingEx.Name = exDto.Name;
                                existingEx.difficulty = exDto.difficulty;
                                existingEx.muscleGroup = exDto.muscleGroup;
                                existingEx.Notes = exDto.Notes;
                            }
                        }
                        else
                        {
                            var newExercise = new WorkoutExercise
                            {
                                Id = Guid.NewGuid(),
                                WorkoutDayId = existingDay.Id,
                                Name = exDto.Name,
                                difficulty = exDto.difficulty,
                                muscleGroup = exDto.muscleGroup,
                                Notes = exDto.Notes
                            };
                            existingDay.Exercises.Add(newExercise);
                        }
                    }
                }
                else
                {
                    var newDay = new WorkoutDay
                    {
                        Id = Guid.NewGuid(),
                        WorkoutPlanId = plan.Id,
                        DayNumber = dayDto.DayNumber,
                        DayName = dayDto.DayName,
                        Notes = dayDto.Notes,
                        Exercises = dayDto.Exercises.Select(exDto => new WorkoutExercise
                        {
                            Id = Guid.NewGuid(),
                            Name = exDto.Name,
                            difficulty = exDto.difficulty,
                            muscleGroup = exDto.muscleGroup,
                            Notes = exDto.Notes
                        }).ToList()
                    };
                    _context.WorkoutDays.Add(newDay);
                }
            }

            await _context.SaveChangesAsync();
            return null;
        }

        public async Task<string?> SubscribeToPlan(SubscribeToPlanDto dto)
        {
            return await _subscriptionService.SubscribeAsync(dto);
        }

        public async Task<string?> UnsubscribeFromPlan(SubscribeToPlanDto dto)
        {
            return await _subscriptionService.UnsubscribeAsync(dto);
        }

        public async Task<int> GetSubscribersCount(Guid planId)
        {
            return await _subscriptionService.GetSubscribersCountAsync(planId);
        }

        public async Task<string?> CreatePlan(CreatePlanHeaderDto dto)
        {
            var coach = await _context.Users.FindAsync(dto.CoachId);
            if (coach == null || coach.Role != UserRole.Coach)
                return "Invalid coach.";

            var plan = new WorkoutPlan
            {
                Id = Guid.NewGuid(),
                PlanName = dto.PlanName,
                PrimaryGoal = dto.PrimaryGoal,
                CoachId = dto.CoachId,
                CreatedAt = DateTime.UtcNow
            };

            _context.WorkoutPlans.Add(plan);
            await _context.SaveChangesAsync();

            return null;
        }

        public async Task<WorkoutPlan?> GetPlanById(Guid planId)
        {
            return await _context.WorkoutPlans
                .Include(p => p.Coach)
                .FirstOrDefaultAsync(p => p.Id == planId);
        }
    }
}
