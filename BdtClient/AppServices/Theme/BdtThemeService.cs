﻿namespace BdtClient.AppServices.Theme;

public class BdtThemeService
{
    public event Action OnThemeChanged;

    public void NotifyThemeChanged()
    {
        OnThemeChanged?.Invoke();
    }
}