namespace SolarMapperUI
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
            //Application.Run(new OpeningForm());
            //Application.Run(new SolarMapperMainForm());
            var openingForm = new OpeningForm();
            if (openingForm.ShowDialog() != DialogResult.OK)
                return;

            var mapSettingsForm = new MapSettingsForm();
            if (mapSettingsForm.ShowDialog() != DialogResult.OK)
                return;
            var generalSettings = mapSettingsForm.GeneralMapSettings;

            Dictionary<string,TypeSettings> typeSettingsDictionary = new Dictionary<string,TypeSettings>();

            foreach ( var typeName in generalSettings.ObjectTypes)
            {
                var typeForm = new TypeFilterForm(typeName);
                if (typeForm.ShowDialog() != DialogResult.OK)
                    return;
                typeSettingsDictionary[typeName] = typeForm.TypeSettings!;
            }
            var mainForm = new SolarMapperMainForm(generalSettings);
            mainForm.ShowDialog();

        }
    }
}