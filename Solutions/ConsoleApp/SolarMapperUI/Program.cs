using SolarSystemMapper;
using System.Diagnostics;

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
            ApplicationConfiguration.Initialize();
            
            var openingForm = new OpeningForm();
            if (openingForm.ShowDialog() != DialogResult.OK)
                return;

            var mapSettingsForm = new MapSettingsForm();
            if (mapSettingsForm.ShowDialog() != DialogResult.OK)
                return;
            var generalSettings = mapSettingsForm.GeneralMapSettings;

            if (generalSettings.MapType == MapType.NightSky)
            {
                SetUpMainForm<EphemerisObserverData>(generalSettings);
            }
            else if (generalSettings.MapType == MapType.SolarSystem)
            {
                SetUpMainForm<EphemerisVectorData>(generalSettings);
            }

            

        }

        internal static void SetUpMainForm<TData>(GeneralMapSettings settings)
            where TData : IEphemerisData<IEphemerisTableRow>
        {
            var typeSettings = new List<TypeSettings<TData>>();


            foreach (var typeName in settings.ObjectTypes)
            {
                var typeForm = new TypeFilterForm<TData>(typeName);
                if (typeForm.ShowDialog() != DialogResult.OK)
                    return;
                typeSettings.Add(typeForm.TypeSettings!);
            }
            Panel panel = new Panel();

            if (typeSettings is List<TypeSettings<EphemerisObserverData>> typeSettingsObserver)
            {
                panel = new NightSkyMapPanel(settings, typeSettingsObserver.Select(x => x.linqFilter));
            }
            if (typeSettings is List<TypeSettings<EphemerisVectorData>> typeSettingsVector)
            {
                panel = new SolarSystemMapPanel(settings, typeSettingsVector.Select(x => x.linqFilter));
            }

            var mainForm = new SolarMapperMainForm<TData>(settings, typeSettings, panel);
            mainForm.ShowDialog();
        }

        


    }
}