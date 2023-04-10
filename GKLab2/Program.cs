namespace GKLab2
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            Pages.MainPage mainPage = new Pages.MainPage();
            Model.GenerelModel generelModel = new Model.GenerelModel(mainPage.CanvasWidht, mainPage.CanvasHeight);
            //Model.GenerelModel generelModel = new Model.GenerelModel();
            Presenter.Presenter presenter = new Presenter.Presenter(generelModel, mainPage);

            Application.Run(mainPage);
        }
    }
}