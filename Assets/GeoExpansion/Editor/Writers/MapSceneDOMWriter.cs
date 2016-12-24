﻿using UnityEngine;
using System.Collections;
using System;

using uAdventure.Editor;
using System.Xml;

namespace uAdventure.Geo
{
    [DOMWriter(typeof(MapScene))]
    public class MapSceneDOMWriter : ParametrizedDOMWriter
    {
        protected override void FillNode(XmlNode node, object target, params IDOMWriterParam[] options)
        {
            var mapScene = target as MapScene;
            var element = node as XmlElement;

            element.SetAttribute("id", mapScene.Id);
            element.SetAttribute("cameraType", mapScene.CameraType.ToString());

            foreach(var mapElement in mapScene.Elements)
            {
                DumpMapElement(node, mapElement, options);   
            }
        }


        protected void DumpMapElement(XmlNode node, MapElement mapElement, params IDOMWriterParam[] options)
        {
            var mapElementNode = Writer.GetDoc().CreateElement("map-element");
            mapElementNode.SetAttribute("targetId", mapElement.getTargetId());
            mapElementNode.SetAttribute("layer", mapElement.Layer.ToString());
            node.AppendChild(mapElementNode);

            if(mapElement is ExtElemReference)
            {
                var elemRef = mapElement as ExtElemReference;
                var elemRefNode = Writer.GetDoc().CreateElement("ext-elem-ref");
                mapElementNode.AppendChild(elemRefNode);

                elemRefNode.SetAttribute("transformManager", elemRef.TransformManagerDescriptor.GetType().ToString());
                foreach(var param in elemRef.TransformManagerDescriptor.ParameterDescription)
                {
                    var paramElem = Writer.GetDoc().CreateElement("param");
                    paramElem.SetAttribute("name", param.Key);
                    paramElem.InnerText = elemRef.TransformManagerParameters[param.Key].ToString();
                    elemRefNode.AppendChild(paramElem);
                }
            }
        }

        protected override string GetElementNameFor(object target)
        {
            return "map-scene";
        }
    }
}

