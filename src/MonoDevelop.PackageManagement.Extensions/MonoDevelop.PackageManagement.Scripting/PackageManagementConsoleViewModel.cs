﻿// 
// PackageManagementConsoleViewModel.cs
// 
// Author:
//   Matt Ward <ward.matt@gmail.com>
// 
// Copyright (C) 2014 Matthew Ward
// 
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

using ICSharpCode.Scripting;
using MonoDevelop.Ide;
using MonoDevelop.PackageManagement;
using MonoDevelop.Projects;
using NuGet.Configuration;

namespace ICSharpCode.PackageManagement.Scripting
{
	internal class PackageManagementConsoleViewModel : ViewModelBase<PackageManagementConsoleViewModel>
	{
		RegisteredPackageSources registeredPackageSources;
		IPackageManagementProjectService projectService;
		IPackageManagementConsoleHost consoleHost;

		DelegateCommand clearConsoleCommand;

		ObservableCollection<SourceRepositoryViewModel> packageSources = new ObservableCollection<SourceRepositoryViewModel> ();
		SourceRepositoryViewModel activePackageSource;

		IScriptingConsole packageManagementConsole;

		public PackageManagementConsoleViewModel (
			RegisteredPackageSources registeredPackageSources,
			IPackageManagementProjectService projectService,
			IPackageManagementConsoleHost consoleHost)
		{
			this.registeredPackageSources = registeredPackageSources;
			this.projectService = projectService;
			this.consoleHost = consoleHost;
		}

		public void RegisterConsole (IScriptingConsole console)
		{
			packageManagementConsole = console;

			IdeApp.Workspace.SolutionLoaded += SolutionLoaded;
			IdeApp.Workspace.SolutionUnloaded += SolutionUnloaded;
			projects = new ObservableCollection<Project> (projectService.GetOpenProjects ().Select (p => p.DotNetProject));

			CreateCommands ();
			UpdatePackageSourceViewModels ();
			ReceiveNotificationsWhenPackageSourcesUpdated ();
			UpdateDefaultProject ();
			InitConsoleHost ();
		}

		void SolutionUnloaded (object sender, SolutionEventArgs e)
		{
			ProjectsChanged (new Project[0]);
		}

		void SolutionLoaded (object sender, SolutionEventArgs e)
		{
			ProjectsChanged (e.Solution.GetAllProjects ().OfType<DotNetProject> ());
		}

		void InitConsoleHost ()
		{
			consoleHost.ScriptingConsole = packageManagementConsole;
			consoleHost.Run ();
		}

		void CreateCommands ()
		{
			clearConsoleCommand = new DelegateCommand (param => ClearConsole ());
		}

		public ICommand ClearConsoleCommand {
			get { return clearConsoleCommand; }
		}

		public void ClearConsole ()
		{
			consoleHost.Clear ();
		}

		void UpdatePackageSourceViewModels ()
		{
			packageSources.Clear ();
			AddEnabledPackageSourceViewModels ();
			SelectActivePackageSource ();
		}

		void AddEnabledPackageSourceViewModels ()
		{
			foreach (SourceRepositoryViewModel packageSource in registeredPackageSources.PackageSources) {
				AddPackageSourceViewModel (packageSource);
			}
		}

		void AddPackageSourceViewModel (SourceRepositoryViewModel packageSource)
		{
			packageSources.Add (packageSource);
		}

		void SelectActivePackageSource ()
		{
			SourceRepositoryViewModel activePackageSource = consoleHost.ActivePackageSource;
			foreach (SourceRepositoryViewModel packageSourceViewModel in packageSources) {
				if (packageSourceViewModel.PackageSource.Equals (activePackageSource)) {
					ActivePackageSource = packageSourceViewModel;
					return;
				}
			}

			SelectFirstActivePackageSource ();
		}

		void SelectFirstActivePackageSource ()
		{
			if (packageSources.Count > 0) {
				ActivePackageSource = packageSources [0];
			}
		}

		void ReceiveNotificationsWhenPackageSourcesUpdated ()
		{
			//registeredPackageSources.CollectionChanged += PackageSourcesChanged;
		}

		void PackageSourcesChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			UpdatePackageSourceViewModels ();
		}

		void UpdateDefaultProject ()
		{
			DefaultProject = this.Projects.FirstOrDefault ();
		}

		void ProjectsChanged (IEnumerable<Project> projects)
		{
			Projects.Clear ();
			NuGet.CollectionExtensions.AddRange (Projects, projects);
			UpdateDefaultProject ();
		}

		public ObservableCollection<SourceRepositoryViewModel> PackageSources {
			get { return packageSources; }
		}

		public SourceRepositoryViewModel ActivePackageSource {
			get { return activePackageSource; }
			set {
				activePackageSource = value;
				consoleHost.ActivePackageSource = activePackageSource;
				OnPropertyChanged (viewModel => viewModel.ActivePackageSource);
			}
		}

		ObservableCollection<Project> projects;

		public ObservableCollection<Project> Projects {
			get { return projects; }
		}

		public Project DefaultProject {
			get { return consoleHost.DefaultProject; }
			set {
				consoleHost.DefaultProject = value;
				OnPropertyChanged (viewModel => viewModel.DefaultProject);
			}
		}

//		public TextEditor TextEditor {
//			get { return packageManagementConsole.TextEditor; }
//		}
//
//		public bool ShutdownConsole()
//		{
//			consoleHost.ShutdownConsole();
//			consoleHost.Dispose();
//			return !consoleHost.IsRunning;
//		}

		public void ProcessUserInput (string line)
		{
			consoleHost.ProcessUserInput (line);
		}
	}
}
