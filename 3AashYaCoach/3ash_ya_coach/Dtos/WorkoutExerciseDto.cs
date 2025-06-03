namespace _3AashYaCoach._3ash_ya_coach.Dtos
{
    public class WorkoutExerciseDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string muscleGroup { get; set; }
        public string difficulty { get; set; }
        public string? Notes { get; set; }
    }
}
