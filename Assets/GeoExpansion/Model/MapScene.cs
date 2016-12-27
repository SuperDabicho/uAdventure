﻿using UnityEngine;
using System.Collections.Generic;
using uAdventure.Core;
using System;

namespace uAdventure.Geo
{
    public enum CameraType
    {
        Aerial2D, Ortographic3D, Perspective3D
    };

    public class MapScene : IChapterTarget
    {
        //-------------
        // Properties
        //-------------
        public CameraType CameraType { get; set; }
        public string Id { get; set; }
        public List<MapElement> Elements { get; set; }

        /// <summary>
        /// Creates a mapScene using the id. Initializing its elements to an empty list.
        /// </summary>
        /// <param name="id">Id to use</param>
        public MapScene(string id)
        {
            Id = id;
            Elements = new List<MapElement>();
        }

        /// <summary>
        /// ToString writes down the ID
        /// </summary>
        /// <returns>string with the id</returns>
        public override string ToString()
        {
            return Id;
        }

        public string getId()
        {
            return Id;
        }

        public string getXApiClass()
        {
            throw new NotImplementedException();
        }

        public string getXApiType()
        {
            throw new NotImplementedException();
        }
    }

}

