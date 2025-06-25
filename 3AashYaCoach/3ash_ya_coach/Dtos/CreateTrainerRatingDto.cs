using System.ComponentModel.DataAnnotations;

namespace _3AashYaCoach._3ash_ya_coach.Dtos
{
    public class CreateTrainerRatingDto
    {
        public Guid CoachId { get; set; }
        [Range(1, 5)]
        public double RatingValue { get; set; }
    }
}
