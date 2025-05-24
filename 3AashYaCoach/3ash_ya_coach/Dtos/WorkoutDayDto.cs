namespace _3AashYaCoach._3ash_ya_coach.Dtos
{
    public class WorkoutDayDto
    {
        public int DayNumber { get; set; }
        public string DayName { get; set; }
        public string? Notes { get; set; }
        public List<WorkoutExerciseDto> Exercises { get; set; }
    }
}
