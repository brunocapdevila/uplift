// --- BEGIN LICENSE BLOCK ---
/*
 * Copyright (c) 2019-present WeWantToKnow AS
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
// --- END LICENSE BLOCK ---


using Uplift.Common;
using Uplift.Packages;
using Uplift.Schemas;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System;

namespace Uplift.DependencyResolution
{
	public class Conflict
	{
		public DependencyDefinition requirement;
		public DependencyGraph activated;

		public Conflict(DependencyDefinition requirement, DependencyGraph activated)
		{
			this.requirement = requirement;
			this.activated = activated;
		}

		public List<string> FindConflictingDependency()
		{
			List<string> conflictingDependencies = new List<string>();
			DependencyNode correspondingNode = this.activated.FindByName(this.requirement.Name);

			DependencyNode copiedNode = new DependencyNode();
			copiedNode.restrictions = correspondingNode.restrictions;
			if (copiedNode.restrictions.Keys != null && copiedNode.restrictions.Keys.Count > 0)
			{
				List<String> restrictors = new List<string>(copiedNode.restrictions.Keys);

				foreach (string key in restrictors)
				{
					//Initial & Legacy requirement cannot be changed
					if (key == "initial" || key == "legacy")
					{
						continue;
					}

					IVersionRequirement tmpVersion = copiedNode.restrictions[key];
					copiedNode.restrictions[key] = new NoRequirement();
					try
					{
						IVersionRequirement newRequirementForNode = copiedNode.ComputeRequirement();

						foreach (PackageRepo package in correspondingNode.selectedPossibilitySet.packages)
						{
							if (newRequirementForNode.IsMetBy(package.Package.PackageVersion))
							{
								Debug.Log("When removing " + key + " requirements on " + this.requirement.Name + " no conflict remains.");
								conflictingDependencies.Add(key);
								break;
							}
						}
					}
					catch (IncompatibleRequirementException e)
					{
						Debug.Log(e.ToString());
						conflictingDependencies.Add(key);
						break;
					}
					finally
					{
						copiedNode.restrictions[key] = tmpVersion;
					}
				}
			}
			return conflictingDependencies;
		}

		override public string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(requirement.Name);
			sb.Append(" ");
			sb.Append(requirement.Version);
			sb.AppendLine(" conflicts with state :");
			sb.AppendLine(activated.ToString());
			return sb.ToString();
		}
	}
}