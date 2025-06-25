using _3AashYaCoach._3ash_ya_coach.Services.DeviceTokenService;
using _3AashYaCoach._3ash_ya_coach.Services.LoginService;
using _3AashYaCoach._3ash_ya_coach.Services.NotificationService.Firebase;
using _3AashYaCoach._3ash_ya_coach.Services.PlanSubscriptionService;
using _3AashYaCoach._3ash_ya_coach.Services.SavedCoachService;
using _3AashYaCoach._3ash_ya_coach.Services.SubscriptionService;
using _3AashYaCoach._3ash_ya_coach.Services.Trainers;
using _3AashYaCoach._3ash_ya_coach.Services.UserInfoService;
using _3AashYaCoach._3ash_ya_coach.Services.WorkoutPlanService;
using _3AashYaCoach._3ash_ya_coach.Services;
using _3AashYaCoach._3ash_ya_coach.Shared;
using Microsoft.AspNetCore.SignalR;

namespace _3AashYaCoach._3ash_ya_coach.Core.DI
{
    public static class ServiceExtensions
    {
        public static void RegisterCustomServices(this IServiceCollection services)
        {
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<ISavedCoachService, SavedCoachService>();
            services.AddScoped<IUserInfoService, UserInfoService>();
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<ITrainerService, TrainerService>();
            services.AddScoped<IPlanSubscriptionService, PlanSubscriptionService>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();
            services.AddScoped<IWorkoutPlanService, WorkoutPlanService>();
            services.AddScoped<IFirebaseNotificationService, FirebaseNotificationService>();
            services.AddScoped<IDeviceTokenService, DeviceTokenService>();
            services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
        }
    }
}
