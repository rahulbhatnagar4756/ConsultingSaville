using OneCoreAssessUI.Enumerables;

namespace OneCoreAssessUI.Services;

/// <summary>
/// what do we need to control?
/// the width of the controls that use the sidebar
/// the content of the sidebar
/// the visibility of the sidebar
/// </summary>
public class RightSidebarService
{
    /// <summary>
    /// int? is the content to be displayed
    /// string is the width of the content
    /// </summary>
    public event Action<bool, ControlTypes, string>? OnToggleSidebar;
    public event Action OnOpenSidebar;

    // Data operation events - simplified and clean
    public event Action<ControlTypes> OnDataRefresh;
    public event Action<ControlTypes> OnDataUpdate;
    public event Action<ControlTypes> OnDataDeleted;
    public event Action<ControlTypes> OnDataEdit;

    public bool isSidebarOpen { get; set; } = false;
    public ControlTypes Control { get; set; } = ControlTypes.none;
    public string ControlWidth { get; set; } = "600px";

    /// <summary>
    /// Open the sidebar (if not already open)
    /// </summary>
    private void ControlSidebar(bool isOpen, ControlTypes control)
    {
        isSidebarOpen = isOpen;
        if (control != Control) 
        { 
            ControlWidth = control.GetControlTypeWidth();
            Control = control; 
        }
        OnToggleSidebar?.Invoke(isSidebarOpen, control, ControlWidth);
    }

    private void ControlSidebar()
    {
        isSidebarOpen = true;
        OnOpenSidebar?.Invoke();
    }

    /// <summary>
    /// open the side bar and load a control with the required width for that control
    /// </summary>
    /// <param name="control"></param>
    public void Open(ControlTypes control, bool isClearSharedContext = false)
    {
        if(isClearSharedContext) SharedContext = null;
        ControlSidebar(true, control);
    }

    public void Open(ControlTypes control, object sharedContext)
    {
        SharedContext = sharedContext;
        ControlSidebar(true, control);
    }

    public void Open() => 
        ControlSidebar();

    /// <summary>
    /// Close the sidebar
    /// </summary>
    public void Close(ControlTypes control = ControlTypes.none) =>
        ControlSidebar(false, control);

    /// <summary>
    /// Set new content for the sidebar
    /// </summary>
    /// <param name="content"></param>
    public void SetContent(ControlTypes control) =>
        ControlSidebar(isSidebarOpen, control);

    // Data operation trigger methods
    
    /// <summary>
    /// Trigger a data refresh for the specified control type
    /// </summary>
    /// <param name="controlType">The control type that needs to refresh its data</param>
    public void TriggerDataRefresh(ControlTypes controlType)
    {
        OnDataRefresh?.Invoke(controlType);
    }

    /// <summary>
    /// Trigger a data update notification for the specified control type
    /// </summary>
    /// <param name="controlType">The control type that has been updated</param>
    public void TriggerDataUpdate(ControlTypes controlType)
    {
        OnDataUpdate?.Invoke(controlType);
    }

    /// <summary>
    /// Trigger a data deletion notification for the specified control type
    /// </summary>
    /// <param name="controlType">The control type that has had data deleted</param>
    public void TriggerDataDeleted(ControlTypes controlType)
    {
        OnDataDeleted?.Invoke(controlType);
    }

    /// <summary>
    /// Trigger an edit mode notification for the specified control type
    /// </summary>
    /// <param name="controlType">The control type entering edit mode</param>
    public void TriggerDataEdit(ControlTypes controlType)
    {
        OnDataEdit?.Invoke(controlType);
    }

    /// <summary>
    /// Trigger multiple events in sequence (useful for operations that affect multiple areas)
    /// </summary>
    /// <param name="controlType">The control type affected</param>
    /// <param name="operations">The operations to trigger</param>
    public void TriggerMultipleEvents(ControlTypes controlType, params DataOperation[] operations)
    {
        foreach (var operation in operations)
        {
            switch (operation)
            {
                case DataOperation.Refresh:
                    TriggerDataRefresh(controlType);
                    break;
                case DataOperation.Update:
                    TriggerDataUpdate(controlType);
                    break;
                case DataOperation.Delete:
                    TriggerDataDeleted(controlType);
                    break;
                case DataOperation.Edit:
                    TriggerDataEdit(controlType);
                    break;
            }
        }
    }

    public object? SharedContext { get; set; }

    public T? GetSharedContext<T>() where T : notnull
    {
        if (SharedContext is T typedContext)
            return typedContext;

        return default;
    }

    /// <summary>
    /// Set the shared context for cross-component communication
    /// </summary>
    /// <param name="context">The context object to share</param>
    public void SetSharedContext(object context)
    {
        SharedContext = context;
    }
}

/// <summary>
/// Enumeration for data operations to use with TriggerMultipleEvents
/// </summary>
public enum DataOperation
{
    Refresh,
    Update,
    Delete,
    Edit
}

