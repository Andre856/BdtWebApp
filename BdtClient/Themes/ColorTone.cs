﻿namespace BdtClient.Themes;

public class ColorTone
{
    public ColorTone(bool dark)
    {
        if (dark)
        {
            Black = "#27272fff";
            White = "#ffffffff";
            Primary = "#776be7ff";
            PrimaryContrastText = "#ffffffff";
            Secondary = "#ff4081ff";
            SecondaryContrastText = "#ffffffff";
            Tertiary = "#1ec8a5ff";
            TertiaryContrastText = "#ffffffff";
            Info = "#3299ffff";
            InfoContrastText = "#ffffffff";
            Success = "#0bba83ff";
            SuccessContrastText = "#ffffffff";
            Warning = "#ffa800ff";
            WarningContrastText = "#ffffffff";
            Error = "#f64e62ff";
            ErrorContrastText = "#ffffffff";
            Dark = "#27272fff";
            DarkContrastText = "#ffffffff";
            TextPrimary = "#ffffffb2";
            TextSecondary = "#ffffff7f";
            TextDisabled = "#ffffff33";
            ActionDefault = "#adadb1ff";
            ActionDisabled = "#ffffff42";
            ActionDisabledBackground = "#ffffff1e";
            Background = "#32333dff";
            BackgroundGrey = "#27272fff";
            Surface = "#373740ff";
            DrawerBackground = "#27272fff";
            DrawerText = "#ffffff7f";
            DrawerIcon = "#ffffff7f";
            AppbarBackground = "#27272fff";
            AppbarText = "#ffffffb2";
            LinesDefault = "#ffffff1e";
            LinesInputs = "#ffffff4c";
            TableLines = "#ffffff1e";
            TableStriped = "#ffffff33";
            TableHover = "#0000000a";
            Divider = "#ffffff1e";
            DividerLight = "#ffffff0f";
            PrimaryDarken = "rgb(90,75,226)";
            PrimaryLighten = "rgb(151,141,236)";
            SecondaryDarken = "rgb(255,31,105)";
            SecondaryLighten = "rgb(255,102,153)";
            TertiaryDarken = "rgb(25,169,140)";
            TertiaryLighten = "rgb(42,223,187)";
            InfoDarken = "rgb(10,133,255)";
            InfoLighten = "rgb(92,173,255)";
            SuccessDarken = "rgb(9,154,108)";
            SuccessLighten = "rgb(13,222,156)";
            WarningDarken = "rgb(214,143,0)";
            WarningLighten = "rgb(255,182,36)";
            ErrorDarken = "rgb(244,47,70)";
            ErrorLighten = "rgb(248,119,134)";
            DarkDarken = "rgb(23,23,28)";
            DarkLighten = "rgb(56,56,67)";
            HoverOpacity = 0.06;
            GrayDefault = "#9E9E9E";
            GrayLight = "#BDBDBD";
            GrayLighter = "#E0E0E0";
            GrayDark = "#757575";
            GrayDarker = "#616161";
            OverlayDark = "rgba(33,33,33,0.4980392156862745)";
            OverlayLight = "rgba(255,255,255,0.4980392156862745)";
        }
        else
        {
            Black = "#272c34ff";
            White = "#ffffffff";
            Primary = "#594ae2ff";
            PrimaryContrastText = "#ffffffff";
            Secondary = "#ff4081ff";
            SecondaryContrastText = "#ffffffff";
            Tertiary = "#1ec8a5ff";
            TertiaryContrastText = "#ffffffff";
            Info = "#2196f3ff";
            InfoContrastText = "#ffffffff";
            Success = "#00c853ff";
            SuccessContrastText = "#ffffffff";
            Warning = "#ff9800ff";
            WarningContrastText = "#ffffffff";
            Error = "#f44336ff";
            ErrorContrastText = "#ffffffff";
            Dark = "#424242ff";
            DarkContrastText = "#ffffffff";
            TextPrimary = "#424242ff";
            TextSecondary = "#00000089";
            TextDisabled = "#00000060";
            ActionDefault = "#00000089";
            ActionDisabled = "#00000042";
            ActionDisabledBackground = "#0000001e";
            Background = "#ffffffff";
            BackgroundGrey = "#f5f5f5ff";
            Surface = "#ffffffff";
            DrawerBackground = "#ffffffff";
            DrawerText = "#424242ff";
            DrawerIcon = "#616161ff";
            AppbarBackground = "#594ae2ff";
            AppbarText = "#ffffffff";
            LinesDefault = "#0000001e";
            LinesInputs = "#bdbdbdff";
            TableLines = "#e0e0e0ff";
            TableStriped = "#00000005";
            TableHover = "#0000000a";
            Divider = "#e0e0e0ff";
            DividerLight = "#000000cc";
            PrimaryDarken = "rgb(62,44,221)";
            PrimaryLighten = "rgb(118,106,231)";
            SecondaryDarken = "rgb(255,31,105)";
            SecondaryLighten = "rgb(255,102,153)";
            TertiaryDarken = "rgb(25,169,140)";
            TertiaryLighten = "rgb(42,223,187)";
            InfoDarken = "rgb(12,128,223)";
            InfoLighten = "rgb(71,167,245)";
            SuccessDarken = "rgb(0,163,68)";
            SuccessLighten = "rgb(0,235,98)";
            WarningDarken = "rgb(214,129,0)";
            WarningLighten = "rgb(255,167,36)";
            ErrorDarken = "rgb(242,28,13)";
            ErrorLighten = "rgb(246,96,85)";
            DarkDarken = "rgb(46,46,46)";
            DarkLighten = "rgb(87,87,87)";
            HoverOpacity = 0.06;
            GrayDefault = "#9E9E9E";
            GrayLight = "#BDBDBD";
            GrayLighter = "#E0E0E0";
            GrayDark = "#757575";
            GrayDarker = "#616161";
            OverlayDark = "rgba(33,33,33,0.4980392156862745)";
            OverlayLight = "rgba(255,255,255,0.4980392156862745)";
        }
    }

    public string Black { get; }
    public string White { get; }
    public string Primary { get;  }
    public string PrimaryContrastText { get; }
    public string Secondary { get; }
    public string SecondaryContrastText { get; }
    public string Tertiary { get; }
    public string TertiaryContrastText { get; }
    public string Info { get; }
    public string InfoContrastText { get; }
    public string Success { get; }
    public string SuccessContrastText { get; }
    public string Warning { get; }
    public string WarningContrastText { get; }
    public string Error { get; }
    public string ErrorContrastText { get; }
    public string Dark { get; }
    public string DarkContrastText { get; }
    public string TextPrimary { get; }
    public string TextSecondary { get; }
    public string TextDisabled { get; }
    public string ActionDefault { get; }
    public string ActionDisabled { get; }
    public string ActionDisabledBackground { get; }
    public string Background { get; }
    public string BackgroundGrey { get; }
    public string Surface { get; }
    public string DrawerBackground { get; }
    public string DrawerText { get; }
    public string DrawerIcon { get; }
    public string AppbarBackground { get; }
    public string AppbarText { get; }
    public string LinesDefault { get; }
    public string LinesInputs { get; }
    public string TableLines { get; }
    public string TableStriped { get; }
    public string TableHover { get; }
    public string Divider { get; }
    public string DividerLight { get; }
    public string PrimaryDarken { get; }
    public string PrimaryLighten { get; }
    public string SecondaryDarken { get; }
    public string SecondaryLighten { get; }
    public string TertiaryDarken { get; }
    public string TertiaryLighten { get; }
    public string InfoDarken { get; }
    public string InfoLighten { get; }
    public string SuccessDarken { get; }
    public string SuccessLighten { get; }
    public string WarningDarken { get; }
    public string WarningLighten { get; }
    public string ErrorDarken { get; }
    public string ErrorLighten { get; }
    public string DarkDarken { get; }
    public string DarkLighten { get; }
    public double HoverOpacity { get; }
    public string GrayDefault { get; }
    public string GrayLight { get; }
    public string GrayLighter { get; }
    public string GrayDark { get; }
    public string GrayDarker { get; }
    public string OverlayDark { get; }
    public string OverlayLight { get; }
}
