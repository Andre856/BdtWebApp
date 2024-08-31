using BdtClient.Shared.Dialogs;
using BdtShared.Dtos.WorkoutType;
using BdtShared.Models.Dialogs;
using MudBlazor;

namespace BdtClient.AppServices.Dialog;

public class BdtDialogService : IBdtDialogService
{
    private readonly IDialogService _dialogService;
    public BdtDialogService(IDialogService dialogService)
    {
        _dialogService = dialogService;
    }

    public async Task ShowErrorDialog(string title, string dialogText)
    {
        var parameters = new DialogParameters
                {
                    { nameof(ErrorDialog.ErrorText), dialogText }
                };
        var dialog = _dialogService.Show<ErrorDialog>(title, parameters);
        await dialog.Result;
    }

    public async Task ShowAlertDialog(string title, string dialogText)
    {
        var parameters = new DialogParameters
                {
                    { nameof(ErrorDialog.ErrorText), dialogText }
                };
        var dialog = _dialogService.Show<ErrorDialog>(title, parameters);
        await dialog.Result;
    }

    public async Task ShowSucessDialog(string title, string dialogText)
    {
        var parameters = new DialogParameters
                {
                    { nameof(ErrorDialog.ErrorText), dialogText }
                };
        var dialog = _dialogService.Show<ErrorDialog>(title, parameters);
        await dialog.Result;
    }

    public async Task<string> ResetPasswordDialog(string email)
    {
        var parameters = new DialogParameters { { "Email", email } };
        var dialogResponse = await _dialogService.ShowAsync<ResetPasswordDialog>("Next Workout", parameters);

        return await dialogResponse.GetReturnValueAsync<string>();
    }

    public async Task<bool> DeleteWorkoutDialog(string title = "Delete Workout")
    {
        var dialogResponse = await _dialogService.ShowAsync<DeleteWorkoutDialog>(title);
        var deleteConfirmed = await dialogResponse.GetReturnValueAsync<bool>();
        return deleteConfirmed;
    }

    public async Task<bool> CancelWorkoutDialog(string title = "Cancel Workout")
    {
        var cancelWorkoutDialog = await _dialogService.ShowAsync<CancelWorkoutDialog>(title);
        var cancelWorkout = await cancelWorkoutDialog.GetReturnValueAsync<bool>();
        return cancelWorkout;
    }

    public async Task<bool> CompleteWorkoutDialog(string title = "Complete Workout")
    {
        var completeWorkoutDialog = await _dialogService.ShowAsync<CompleteWorkoutDialog>(title);
        var completeWorkout = await completeWorkoutDialog.GetReturnValueAsync<bool>();
        return completeWorkout;
    }

    public async Task<bool> RestartWorkoutDialog(string title = "Restart Workout")
    {
        var restartWorkoutDialog = await _dialogService.ShowAsync<RestartWorkoutDialog>(title);
        var restartWorkout = await restartWorkoutDialog.GetReturnValueAsync<bool>();
        return restartWorkout;
    }

    public async Task NextWorkoutDashboardDialog(decimal workoutDuration, bool isFromPlanner, string title = "Next Workout")
    {
        var parameters = new DialogParameters
        {
            { "WorkoutDuration", workoutDuration },
            { "IsFromPlanner", isFromPlanner }
        };
        var dialogResponse = await _dialogService.ShowAsync<NextWorkoutDashboardDialog>(title, parameters);
    }

    public async Task<CreatePlanDialogModel> CreatePlanDialog(List<WorkoutTypeDto> workoutTypes, int timeScheduled, string title = "Create Plan")
    {
        var parameters = new DialogParameters
        {
            { "WorkoutTypes", workoutTypes },
            { "TimeScheduled", timeScheduled }
        };
        var dialogResponse = await _dialogService.ShowAsync<CreatePlanDialog>(title, parameters);

        var newWorkout = await dialogResponse.GetReturnValueAsync<CreatePlanDialogModel>();

        return newWorkout;
    }

    public async Task<bool> TermsAndConditionDialog()
    {
        var tsAndCs = await _dialogService.ShowAsync<TermsAndConditionDialog>();
        var tsAndCsOk = await tsAndCs.GetReturnValueAsync<bool>();
        return tsAndCsOk;
    }

    public async Task<bool> CancelSubscriptionDialog()
    {
        var cancelSubscriptionDialog = await _dialogService.ShowAsync<CancelSubscriptionDialog>("Cancel Subscription");
        var cancelSubscription = await cancelSubscriptionDialog.GetReturnValueAsync<bool>();
        return cancelSubscription;
    }

    public async Task<bool> DeleteUserAccountDialog()
    {
        var deleteUserAccountDialog = await _dialogService.ShowAsync<DeleteUserAccountDialog>("Delete Account");
        var deleteUserAccount = await deleteUserAccountDialog.GetReturnValueAsync<bool>();
        return deleteUserAccount;
    }
}
