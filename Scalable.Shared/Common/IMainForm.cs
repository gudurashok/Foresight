namespace Scalable.Shared.Common
{
    public interface IMainForm
    {
        bool IsClosing { get; set; }
        void Show();
        void Close();
    }
}
