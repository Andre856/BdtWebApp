using BdtServer.AppServices.App;
using BdtServer.Themes;
using BdtShared.Models.App;
using MudBlazor;
using Newtonsoft.Json;

namespace BdtServer.AppServices.Theme;

public class ThemeService
{
    public readonly IAppService _appService;

    public event Action? ThemeChanged;
    public MudTheme CurrentTheme { get; set; } = SetDefaultTheme();
    public ColorTone CurrentColorTone { get; set; } = new ColorTone(true);
    public bool IsDarkTheme { get; set; } = false;

    public ThemeService(IAppService appService)
    {
        _appService = appService;
    }

    public async Task ChangeColorTone(bool isDark)
    {
        if (isDark && !Setting.UserBasicDetail.IsFirstLogin)
            CurrentTheme = MudBlazorThemes.DarkTheme;
        else
            CurrentTheme = SetDefaultTheme();

        CurrentColorTone = new ColorTone(isDark);

        IsDarkTheme = isDark;

        await _appService.UpdateUserTheme(isDark);

        Setting.UserBasicDetail.IsDarkMode = isDark;
        string userBasicDetailString = JsonConvert.SerializeObject(Setting.UserBasicDetail);
        //await SecureStorage.SetAsync(nameof(Setting.UserBasicDetail), userBasicDetailString);

        ThemeChanged?.Invoke();
    }

    private static MudTheme SetDefaultTheme()
    {
        var defaultTheme = MudBlazorThemes.LightTheme;
        defaultTheme.PaletteLight.Background = "rgba(240,240,240,1)";
        defaultTheme.PaletteLight.AppbarBackground = "rgba(39,39,47,1);";
        return defaultTheme;
    }

    private static MudTheme SetDarkTheme()
    {
        var darkTheme = MudBlazorThemes.DarkTheme;
        //darkTheme.Palette.AppbarBackground = "rgba(39,39,47,1);"
        return darkTheme;
    }
}
