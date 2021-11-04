namespace WpfTetris.Interfaces
{
    public delegate void CloseControl();

    public interface ICloseControl
    {
        event CloseControl CloseEvent;
    }
}