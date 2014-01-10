using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PlayLibrary
{
    /// <summary>
    /// Interface visant à rendre compatible votre application avec le plateforme Play. Il est nécessaire de tout implémenter via une de vos classes qui lancera la fenêtre de votre application.
    /// </summary>
    public interface IPlayApp
    {
        /// <summary>
        /// La version de l’application
        /// </summary>
        string Version { get; set; }
        /// <summary>
        /// Le nom de l'application
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// La version de l’application
        /// </summary>
        string Description { get; set; }
        /// <summary>
        /// Image de l'application
        /// </summary>
        Bitmap Thumbnail { get; set; }
        /// <summary>
        /// Renseigne si l’application est destinée à être utilisée dans les locaux (false) ou également dehors (true)
        /// </summary>
        bool Mode { get; set; }
        /// <summary>
        /// La date de fin de réalisation de l’application
        /// </summary>
        DateTime Date { get; set; }
        /// <summary>
        /// Le langage principal de l'application
        /// </summary>
        TechnologyType Technology { get; set; }
        /// <summary>
        /// Catégorie de l’application permettant de les classer sur la plateforme
        /// </summary>
        CategoryType Category { get; set; }

        /// <summary>
        /// Méthode permettant de lancer la fenêtre de votre application
        /// </summary>
        bool LaunchApp();
    }
}
