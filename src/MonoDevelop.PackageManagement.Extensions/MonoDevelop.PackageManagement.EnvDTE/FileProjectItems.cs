﻿// 
// FileProjectItems.cs
// 
// Author:
//   Matt Ward <ward.matt@gmail.com>
// 
// Copyright (C) 2012-2014 Matthew Ward
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
using System.IO;
using System.Linq;
using MD = MonoDevelop.Projects;

namespace ICSharpCode.PackageManagement.EnvDTE
{
	/// <summary>
	/// A file can have child project items if it has files that depend upon it.
	/// For example, winform designer files (MainForm.Designer.cs)
	/// </summary>
	public class FileProjectItems : ProjectItems
	{
		ProjectItem projectItem;
		IPackageManagementFileService fileService;

		public FileProjectItems (ProjectItem projectItem)
			: base ((Project)projectItem.ContainingProject, projectItem)
		{
			this.projectItem = projectItem;
			this.fileService = Project.FileService;
		}

		//		protected override IEnumerable<global::EnvDTE.ProjectItem> GetProjectItems ()
		protected override IEnumerable<ProjectItem> GetProjectItems ()
		{
			return GetChildDependentProjectItems ().ToList ();
		}

		IEnumerable<ProjectItem> GetChildDependentProjectItems ()
		{
			foreach (MD.ProjectFile fileProjectItem in GetFileProjectItems()) {
				if (fileProjectItem.IsDependentUpon (projectItem.MSBuildProjectItem)) {
					yield return new ProjectItem (Project, fileProjectItem);
				}
			}
		}

		IEnumerable<MD.ProjectFile> GetFileProjectItems ()
		{
			return Project
				.DotNetProject
				.Items
				.Where (item => item is MD.ProjectFile)
				.Select (item => (MD.ProjectFile)item);
		}

		protected override ProjectItem AddFileProjectItemToProject (string fileName)
		{
			return AddFileProjectItemWithDependent (fileName);
		}

		ProjectItem AddFileProjectItemWithDependent (string fileName)
		{
			return Project.AddFileProjectItemWithDependentUsingFullPath (fileName, projectItem.Name);
		}

		public override string Kind {
			get { return global::EnvDTE.Constants.vsProjectItemKindPhysicalFile; }
		}
	}
}
