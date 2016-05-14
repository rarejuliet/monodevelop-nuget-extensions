﻿// 
// SolutionPackageRepository.cs
// 
// Author:
//   Matt Ward <ward.matt@gmail.com>
// 
// Copyright (C) 2012 Matthew Ward
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
using System.Linq;

using MonoDevelop.PackageManagement;
using MonoDevelop.Projects;
using NuGet;

namespace ICSharpCode.PackageManagement
{
	internal class SolutionPackageRepository2 : ISolutionPackageRepository2
	{
		SolutionPackageRepositoryPath2 repositoryPath;
		IMonoDevelopPackageRepositoryFactory repositoryFactory;
		DefaultPackagePathResolver packagePathResolver;
		PhysicalFileSystem fileSystem;
		ISharedPackageRepository repository;

		public SolutionPackageRepository2 (Solution solution)
			: this (
				solution,
				new MonoDevelopPackageRepositoryFactory (),
				PackageManagementServices.Options)
		{
		}

		public SolutionPackageRepository2 (
			Solution solution,
			IMonoDevelopPackageRepositoryFactory repositoryFactory,
			PackageManagementOptions options)
		{
			this.repositoryFactory = repositoryFactory;
			repositoryPath = new SolutionPackageRepositoryPath2 (solution, options);
			CreatePackagePathResolver ();
			CreateFileSystem ();
			CreateRepository (ConfigSettingsFileSystem2.CreateConfigSettingsFileSystem (solution));
		}

		void CreatePackagePathResolver ()
		{
			packagePathResolver = new DefaultPackagePathResolver (repositoryPath.PackageRepositoryPath);
		}

		void CreateFileSystem ()
		{
			fileSystem = new PhysicalFileSystem (repositoryPath.PackageRepositoryPath);
		}

		void CreateRepository (ConfigSettingsFileSystem2 configSettingsFileSystem)
		{
			repository = repositoryFactory.CreateSharedRepository (packagePathResolver, fileSystem, configSettingsFileSystem);
		}

		public ISharedPackageRepository Repository {
			get { return repository; }
		}

		public IFileSystem FileSystem {
			get { return fileSystem; }
		}

		public IPackagePathResolver PackagePathResolver {
			get { return packagePathResolver; }
		}

		public string GetInstallPath (IPackage package)
		{
			return repositoryPath.GetInstallPath (package);
		}

		public IEnumerable<IPackage> GetPackagesByDependencyOrder ()
		{
			var packageSorter = new PackageSorter (null);
			return packageSorter.GetPackagesByDependencyOrder (repository);
		}

		public IEnumerable<IPackage> GetPackagesByReverseDependencyOrder ()
		{
			return GetPackagesByDependencyOrder ().Reverse ();
		}

		public bool IsInstalled (IPackage package)
		{
			return repository.Exists (package);
		}

		public IQueryable<IPackage> GetPackages ()
		{
			return repository.GetPackages ();
		}
	}
}