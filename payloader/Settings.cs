using System.Configuration;

namespace payloader.Properties {
    internal sealed partial class Settings {
        public Settings() {
        }

        /// <summary>
        /// Store the last valid path.
        /// </summary>
        [UserScopedSetting]
        public string path {
            get { return (string)this["path"]; }
            set { this["path"] = value; }
        }
    }
}
