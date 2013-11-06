using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GP3_Coursework
{
    class ModelManager
    {
        string s_ModelName;
        Dictionary<string, Model> dc_ModelList;


        /// <summary>
        /// Add a model into the model library, this is an attempt to make it easier to access and manipulate the models/
        /// </summary>
        /// <param name="name"> A memorable name to call the model</param>
        /// <param name="modelToAdd"> Takes the model (already loaded in game1.cs and adds to the collection.</param>
        public void addModel(string name,Model modelToAdd)
        {
            dc_ModelList.Add(name, modelToAdd);
        }

        /// <summary>
        /// Retrieve the model from the disctionary
        /// </summary>
        /// <param name="name"> The name to search the dictionary for, this will
        /// match the key value pair from before</param>
        /// <returns> the model from the collection matching the string</returns>
        public void GetModel(string name)
        {
            Model m_tempModel;
           
            if (dc_ModelList.ContainsKey(name))
            {
                m_tempModel = dc_ModelList["name"];
                
            }
            
        }
    }
}
