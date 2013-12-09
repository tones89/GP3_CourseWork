using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace GP3_Coursework
{
    class EffectManager
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Model theMesh;
        Model theOceanMesh;

        Texture2D diffuseOceanTexture;
        Texture2D normalOceanTexture;

        // The object that will contain our shader
        Effect effect;
        Effect oceanEffect;
      
        // Parameters for Ocean shader
        EffectParameter projectionOceanParameter;
        EffectParameter viewOceanParameter;
        EffectParameter worldOceanParameter;
        EffectParameter ambientIntensityOceanParameter;
        EffectParameter ambientColorOceanParameter;

        EffectParameter diffuseIntensityOceanParameter;
        EffectParameter diffuseColorOceanParameter;
        EffectParameter lightDirectionOceanParameter;

        EffectParameter eyePosOceanParameter;
        EffectParameter specularColorOceanParameter;

        EffectParameter colorMapTextureOceanParameter;
        EffectParameter normalMapTextureOceanParameter;
        EffectParameter totalTimeOceanParameter;

        Matrix rotateIslandMatrix;
        Matrix world, view, projection;
        Vector4 ambientLightColor;
        Vector3 eyePos;
        float totalTime = 0.0f;

        ContentManager content;
        GraphicsDevice graphDev;

        public EffectManager(GraphicsDeviceManager Graphics,GraphicsDevice GraphDev, SpriteBatch SpriteBatch,ContentManager Content)
        {
           
            content = Content;
            graphics = Graphics;
            spriteBatch = SpriteBatch;
            graphDev = GraphDev;
        }

        public void LoadContent()
        {
           
            theOceanMesh = content.Load<Model>(".\\Water\\ocean");

            // Load the shader
            oceanEffect = content.Load<Effect>(".\\Water\\OceanShader");

            // Set up the parameters
            SetupOcean();

            // calculate matrixes
            float aspectRatio = (float)graphics.GraphicsDevice.Viewport.Width / (float)graphics.GraphicsDevice.Viewport.Height;
            float fov = MathHelper.PiOver4 * aspectRatio * 3 / 4;
            projection = Matrix.CreatePerspectiveFieldOfView(fov, aspectRatio, 0.1f, 10000.0f);

            diffuseOceanTexture = content.Load<Texture2D>(".\\Water\\water");
            normalOceanTexture = content.Load<Texture2D>(".\\Water\\wavesbump");

            //create a default world matrix
            world = Matrix.Identity;
        }

        private void SetupOcean()
        {

            // Bind the parameters with the shader.
            worldOceanParameter = oceanEffect.Parameters["World"];
            viewOceanParameter = oceanEffect.Parameters["View"];
            projectionOceanParameter = oceanEffect.Parameters["Projection"];

            ambientColorOceanParameter = oceanEffect.Parameters["AmbientColor"];
            ambientIntensityOceanParameter = oceanEffect.Parameters["AmbientIntensity"];

            diffuseColorOceanParameter = oceanEffect.Parameters["DiffuseColor"];
            diffuseIntensityOceanParameter = oceanEffect.Parameters["DiffuseIntensity"];
            lightDirectionOceanParameter = oceanEffect.Parameters["LightDirection"];

            eyePosOceanParameter = oceanEffect.Parameters["EyePosition"];
            specularColorOceanParameter = oceanEffect.Parameters["SpecularColor"];

            colorMapTextureOceanParameter = oceanEffect.Parameters["ColorMap"];
            normalMapTextureOceanParameter = oceanEffect.Parameters["NormalMap"];
            totalTimeOceanParameter = oceanEffect.Parameters["TotalTime"];

        }

        public void DrawOcean(GraphicsDevice graphicsDevice, GameTime gameTime )
        {

            ModelMesh mesh = theOceanMesh.Meshes[0];
            ModelMeshPart meshPart = mesh.MeshParts[0];

            // Set parameters
            projectionOceanParameter.SetValue(projection);
            viewOceanParameter.SetValue(view);
            worldOceanParameter.SetValue(Matrix.CreateRotationY((float)MathHelper.ToRadians((int)270)) 
                * Matrix.CreateRotationZ((float)MathHelper.ToRadians((int)90)) * Matrix.CreateScale(100.0f) 
                * Matrix.CreateTranslation(0, -60, 0)); //Matrix.CreateScale(50.0f) * Matrix.CreateRotationX(MathHelper.ToRadians(270)) * Matrix.CreateTranslation(0, -60, 0);
            ambientIntensityOceanParameter.SetValue(0.4f);
            ambientColorOceanParameter.SetValue(ambientLightColor);
            diffuseColorOceanParameter.SetValue(Color.White.ToVector4());
            diffuseIntensityOceanParameter.SetValue(0.2f);
            specularColorOceanParameter.SetValue(Color.White.ToVector4());
            eyePosOceanParameter.SetValue(eyePos);
            colorMapTextureOceanParameter.SetValue(diffuseOceanTexture);
            normalMapTextureOceanParameter.SetValue(normalOceanTexture);
            totalTimeOceanParameter.SetValue(totalTime);

            Vector3 lightDirection = new Vector3(1.0f, 0.0f, -1.0f);

            //ensure the light direction is normalized, or
            //the shader will give some weird results
            lightDirection.Normalize();
            lightDirectionOceanParameter.SetValue(lightDirection);

            //set the vertex source to the mesh's vertex buffer
            graphics.GraphicsDevice.SetVertexBuffer(meshPart.VertexBuffer, meshPart.VertexOffset);

            //set the current index buffer to the sample mesh's index buffer
            graphics.GraphicsDevice.Indices = meshPart.IndexBuffer;

           graphicsDevice.BlendState = BlendState.AlphaBlend;
           graphicsDevice.DepthStencilState = DepthStencilState.DepthRead;

            oceanEffect.CurrentTechnique = oceanEffect.Techniques["Technique1"];

            for (int i = 0; i < oceanEffect.CurrentTechnique.Passes.Count; i++)
            {
                //EffectPass.Apply will update the device to
                //begin using the state information defined in the current pass
                oceanEffect.CurrentTechnique.Passes[i].Apply();
                //theMesh contains all of the information required to draw
                //the current mesh
               
                    graphicsDevice.DrawIndexedPrimitives(
                    PrimitiveType.TriangleList,0, 0,
                    meshPart.NumVertices, meshPart.StartIndex, meshPart.PrimitiveCount);
            }

        }



    }
}
