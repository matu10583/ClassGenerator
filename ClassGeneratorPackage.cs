using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using Task = System.Threading.Tasks.Task;
using System.ComponentModel;

namespace ClassGenerator
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(ClassGeneratorPackage.PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideOptionPage(typeof(OptionPageGrid), 
        "My Category", "My Grid Page", 0, 0, true)]
    public sealed class ClassGeneratorPackage : AsyncPackage
    {
        /// <summary>
        /// ClassGeneratorPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "0f3849a4-4e3f-427b-8691-00f61a4a73b3";
        public string IncludeBasePath
        {
            get
            {
                OptionPageGrid page = (OptionPageGrid) GetDialogPage(typeof(OptionPageGrid));
                return page.IncludeBasePath;
            }
        }
        public string SrcBasePath
        {
            get
            {
                OptionPageGrid page = (OptionPageGrid)GetDialogPage(typeof(OptionPageGrid));
                return page.SrcBasePath;
            }
        }
        public string SrcBaseFilter
        {
            get
            {
                OptionPageGrid page = (OptionPageGrid)GetDialogPage(typeof(OptionPageGrid));
                return page.SrcBaseFilter;
            }
        }
        public string IncludeBaseFilter
        {
            get
            {
                OptionPageGrid page = (OptionPageGrid)GetDialogPage(typeof(OptionPageGrid));
                return page.IncludeBaseFilter;
            }
        }

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            await Generate.InitializeAsync(this);
        }

        #endregion
    }

    public class OptionPageGrid : DialogPage
    {
        private string includeBasePath = "include";

        [Category("Class Generator")]
        [DisplayName("Include Base Path")]
        [Description("include base path")]
        public string IncludeBasePath
        {

            get { return includeBasePath; }
            set{ includeBasePath = value; }
        }

        private string srcBasePath = "src";

        [Category("Class Generator")]
        [DisplayName("Source Base Path")]
        [Description("source base path")]
        public string SrcBasePath
        {

            get { return srcBasePath; }
            set { srcBasePath = value; }
        }

        private string srcBaseFilter = "ソース ファイル";

        [Category("Class Generator")]
        [DisplayName("Source Base Filter")]
        [Description("source base Filter")]
        public string SrcBaseFilter
        {

            get { return srcBaseFilter; }
            set { srcBaseFilter = value; }
        }

        private string incBaseFilter = "ヘッダー ファイル";

        [Category("Class Generator")]
        [DisplayName("Include Base Filter")]
        [Description("include base Filter")]
        public string IncludeBaseFilter
        {

            get { return incBaseFilter; }
            set { incBaseFilter = value; }
        }
    }
}
