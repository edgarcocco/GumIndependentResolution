using System;
using System.Collections.Generic;
using System.Linq;
using Gum.Wireframe;
using GumRuntime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using MonoGameGum;
using MonoGameGum.Forms;
using MonoGameGum.Forms.Controls;
using RenderingLibrary;
using ResolutionBuddy;

namespace GumIndependentResolution
{
    public class GumIndependentResolutionGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private RenderTarget2D _renderTarget;
        public GumIndependentResolutionGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IResolution resolution = new ResolutionComponent(this, graphics,
                                                    new Point(1024, 768),
                                                    new Point(512, 384), false, false, false);
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
#if (ANDROID || iOS)
            graphics.IsFullScreen = true;
#endif
            IsMouseVisible = true;

        }

        private GraphicalUiElement screen;
        protected override void Initialize()
        {
            _renderTarget = new RenderTarget2D(GraphicsDevice, 1024, 768);
            var gumProject = MonoGameGum.GumService.Default.Initialize(
                                       this.GraphicsDevice,
                           "GumLayout/gumlayout.gumx");

            screen = gumProject.Screens.First().ToGraphicalUiElement(
            SystemManagers.Default, addToManagers: true);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            if (keyboardState.IsKeyDown(Keys.Escape) ||
                keyboardState.IsKeyDown(Keys.Back) ||
                gamePadState.Buttons.Back == ButtonState.Pressed)
            {
                try { Exit(); }
                catch (PlatformNotSupportedException) { /* ignore */ }
            }

            // TODO: Add your update logic here
            GumService.Default.Update(this, gameTime, screen);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.SetRenderTarget(_renderTarget);
            GumService.Default.Draw();
            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, transformMatrix: ResolutionBuddy.Resolution.TransformationMatrix());
            spriteBatch.Draw(_renderTarget, Vector2.Zero, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
