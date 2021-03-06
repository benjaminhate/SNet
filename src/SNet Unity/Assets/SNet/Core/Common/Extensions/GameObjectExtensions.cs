﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SNet.Core.Common.Extensions
{
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Get a component in a child with a tag
        /// This child must have a specific tag
        /// </summary>
        /// <param name="parent">The GameObject parent of the child</param>
        /// <param name="tag">The tag to filter</param>
        /// <typeparam name="T">The type of the component</typeparam>
        /// <returns>The component of the child if found; null if not</returns>
        public static T GetComponentInChildWithTag<T>(this GameObject parent, string tag) where T:Component
        {
            var t = parent.transform;
            return (from Transform tr in t where tr.CompareTag(tag) select tr.GetComponent<T>()).FirstOrDefault();
        }
        
        /// <summary>
        /// Get the list of components in the children with a tag
        /// The children must have a specific tag
        /// </summary>
        /// <param name="parent">The GameObject parent of the children</param>
        /// <param name="tag">The tag to filter</param>
        /// <typeparam name="T">The type of the component</typeparam>
        /// <returns>The list of components found</returns>
        public static List<T> GetComponentsInChildrenWithTag<T>(this GameObject parent, string tag) where T:Component
        {
            var t = parent.transform;
            return (from Transform tr in t where tr.CompareTag(tag) select tr.GetComponent<T>()).ToList();
        }
        
        /// <summary>
        /// Get a child with a specific tag
        /// </summary>
        /// <param name="parent">The GameObject parent of the child</param>
        /// <param name="tag">The tag to filter</param>
        /// <returns>The GameObject child if found; null if not</returns>
        public static GameObject GetChildWithTag(this GameObject parent, string tag)
        {
            var t = parent.transform;

            return (from Transform tr in t where tr.CompareTag(tag) select tr.gameObject).FirstOrDefault();
        }

        /// <summary>
        /// Get the children with a specific tag
        /// </summary>
        /// <param name="parent">The GameObject parent of the children</param>
        /// <param name="tag">the tag to filter</param>
        /// <returns>The list of GameObject children found</returns>
        public static List<GameObject> GetChildrenWithTag(this GameObject parent, string tag)
        {
            var t = parent.transform;

            return (from Transform tr in t where tr.CompareTag(tag) select tr.gameObject).ToList();
        }

        public static void ChildrenSetActive(this GameObject parent, bool enabled)
        {
            var t = parent.transform;
            foreach (Transform child in t)
            {
                child.gameObject.SetActive(enabled);
            }
        }
    }
}
