using Bdt.Shared.Dtos.WorkoutType;
using Bdt.Shared.Models.Dialogs;

namespace Bdt.Client.AppServices.Dialog;

public interface IBdtDialogService
{
    Task ShowErrorDialog(string title, string dialogText);
    Task ShowAlertDialog(string title, string dialogText);
    Task ShowSucessDialog(string title, string dialogText);
    Task<string> ResetPasswordDialog(string email);
    Task<bool> DeleteWorkoutDialog(string title = "Delete Workout");
    Task<bool> CancelWorkoutDialog(string title = "Cancel Workout");
    Task<bool> CompleteWorkoutDialog(string title = "Complete Workout");
    Task<bool> RestartWorkoutDialog(string title = "Restart Workout");
    Task NextWorkoutDashboardDialog(decimal workoutDuration, bool isFromPlanner, string title = "Next Workout");
    Task<CreatePlanDialogModel> CreatePlanDialog(List<WorkoutTypeDto> workoutTypes, int timeScheduled, string title = "Create Plan");
    Task<bool> TermsAndConditionDialog();
    Task<bool> CancelSubscriptionDialog();
    Task<bool> DeleteUserAccountDialog();
}
