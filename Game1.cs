using System.Collections.Generic;
using System.Collections;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SnakeMain {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D bgImage;
        Texture2D head, body, end;
        Texture2D food;
        Texture2D start, win, lose;

        SpriteFont font;

        LinkedList<Vector2> bits;
        int width, height;
        int score;
        double timer;
        bool gameStart;
        bool gameOver;
        bool won;
        KeyboardState oldState;
        Vector2 direction;
        Vector2 lastDirection;
        Vector2 foodPlace;


        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.Window.Title = "Snake, eh";
            graphics.PreferredBackBufferWidth = 512;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 256;   // set this value to the desired height of your window
            graphics.ApplyChanges();
        }

        public void StartGame() {
            width = 16;
            height = 8;
            gameOver = false;
            won = false;

            bits = new LinkedList<Vector2>();
            bits.AddFirst(new Vector2(0, 0));

            timer = 0f;
            score = 0;
            direction = new Vector2(1, 0);
            foodPlace = new Vector2(6, 6);
        }
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            StartGame();
            gameStart = false;
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            bgImage = Content.Load<Texture2D>("Sprites/Background");
            head = Content.Load<Texture2D>("Sprites/Head");
            body = Content.Load<Texture2D>("Sprites/Body");
            end = Content.Load<Texture2D>("Sprites/End");
            food = Content.Load<Texture2D>("Sprites/Food");

            start = Content.Load<Texture2D>("Sprites/start");
            win = Content.Load<Texture2D>("Sprites/win");
            lose = Content.Load<Texture2D>("Sprites/lose");

            font = Content.Load<SpriteFont>("Sprites/sharpRetro");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            timer += gameTime.ElapsedGameTime.TotalSeconds;
            lastDirection = direction;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape)) {
                Exit();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Space)){
                StartGame();
                gameStart = true;
            }

            KeyboardState newState = Keyboard.GetState();

            if (!gameOver && !won && gameStart) {
                if (oldState.IsKeyUp(Keys.S) && newState.IsKeyDown(Keys.S)
                    && !(bits.Contains(bits.First() + new Vector2(0, 1)))) {
                    direction = new Vector2(0, 1);
                    Move();
                }
                else if (oldState.IsKeyUp(Keys.W) && newState.IsKeyDown(Keys.W)
                    && !(bits.Contains(bits.First() + new Vector2(0, -1)))) {
                    direction = new Vector2(0, -1);
                    Move();
                }
                else if (oldState.IsKeyUp(Keys.A) && newState.IsKeyDown(Keys.A)
                    && !(bits.Contains(bits.First() + new Vector2(-1, 0)))) {
                    direction = new Vector2(-1, 0);
                    Move();
                }
                else if (oldState.IsKeyUp(Keys.D) && newState.IsKeyDown(Keys.D)
                    && !(bits.Contains(bits.First() + new Vector2(1, 0)))) {
                    direction = new Vector2(1, 0);
                    Move();
                }
                else if (timer >= 0.2) {
                    timer -= 0.2;
                    Move();
                }
                oldState = newState;
            }

            // TODO: Add your update logic here

            

            base.Update(gameTime);
        }

        void Move() {
            Vector2 tempBit = bits.First() + direction;
            if (tempBit.X <= width - 1
                && tempBit.Y <= height - 1
                && tempBit.X >= 0
                && tempBit.Y >= 0
                && bits.Contains(tempBit) == false) {
                bits.RemoveLast();
                bits.AddFirst(tempBit);
            }
            else {
                gameOver = true;
            }

            if (tempBit == foodPlace) {

                if ((bits.Last() + direction * (-1)).Y < height
                    && (bits.First() + direction * (-1)).Y >= 0
                    && (bits.First() + direction * (-1)).X < width
                    && (bits.First() + direction * (-1)).X >= 0) {
                    bits.AddLast(bits.First() + direction * (-1));
                }
                else if ((bits.First() + new Vector2(0, -1)).Y >= 0
                    && !(bits.Contains(bits.First() + new Vector2(0, -1)))) {
                    bits.AddLast(bits.First() + new Vector2(0, -1));
                }
                else if ((bits.First() + new Vector2(0, 1)).Y < height
                    && !(bits.Contains(bits.First() + new Vector2(0, 1)))) {
                    bits.AddLast(bits.First() + new Vector2(0, 1));
                }
                else if ((bits.First() + new Vector2(1, 0)).X < width
                    && !(bits.Contains(bits.First() + new Vector2(1, 0)))) {
                    bits.AddLast(bits.First() + new Vector2(1, 0));
                }
                else if (((bits.First() + new Vector2(-1, 0)).X >= 0)
                    && !(bits.Contains(bits.First() + new Vector2(-1, 0)))) {
                    bits.AddLast(bits.First() + new Vector2(-1, 0));
                }
                else {
                    gameOver = true;
                }

                Random r = new Random();
                int posFoodPlaceX = r.Next(width);
                int posFoodPlaceY = r.Next(height);
                while (bits.Contains(new Vector2(posFoodPlaceX, posFoodPlaceY))) {
                    posFoodPlaceX = r.Next(width);
                    posFoodPlaceY = r.Next(height);
                }
                foodPlace = new Vector2(posFoodPlaceX, posFoodPlaceY);
                score += 100;
                if (bits.Count == height * width) {
                    won = true;
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.Draw(start, new Vector2(0, 0), Color.White);
            if (!gameOver && !won && gameStart) {
                for (int i = 0; i < width; i++) {
                    for (int j = 0; j < height; j++) {
                        spriteBatch.Draw(bgImage, new Vector2(i * 32f, j * 32f), Color.White);
                    }
                }
                Vector2 halfBlock = new Vector2(16, 16);

                float rotation = 0;

                if (bits.Count > 1) {
                    for (int i = 1; i < bits.Count - 1; i++) {
                        spriteBatch.Draw(body, bits.ElementAt(i) * 32f, Color.White);
                    }
                    rotation = (float)Math.Atan2((double)(bits.ElementAt(bits.Count - 2).Y - bits.Last().Y), (double)(bits.ElementAt(bits.Count - 2).X) - bits.Last().X);
                    spriteBatch.Draw(end, bits.Last() * 32f + halfBlock, null, Color.White, rotation, halfBlock + new Vector2(end.Width / 2, end.Height / 2) - halfBlock, 1.0f, SpriteEffects.None, 1.0f);
                }


                rotation = (float)Math.Atan2((double)lastDirection.Y, (double)lastDirection.X);
                spriteBatch.Draw(head, bits.First() * 32f + halfBlock, null, Color.White, rotation, halfBlock + new Vector2(head.Width / 2, head.Height / 2) - halfBlock, 1.0f, SpriteEffects.None, 1.0f);

                spriteBatch.Draw(food, foodPlace * 32f, Color.White);

                Vector2 textMiddlePoint = font.MeasureString(score.ToString()) / 2;
                // Places text in center of the screen
                Vector2 fontPos = new Vector2(8, 8) + textMiddlePoint;
                spriteBatch.DrawString(font, score.ToString(), fontPos, Color.DarkSlateGray, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            }
            else if (gameOver) {
                spriteBatch.Draw(lose, new Vector2(0, 0), Color.White);
                Vector2 textMiddlePoint = font.MeasureString(score.ToString()) / 2;
                Vector2 fontPos = new Vector2(256, 128);
                spriteBatch.DrawString(font, score.ToString(), fontPos, Color.GhostWhite, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
                
            }
            else if (won) {
                spriteBatch.Draw(win, new Vector2(0, 0), Color.White);
                Vector2 textMiddlePoint = font.MeasureString(score.ToString()) / 2;
                Vector2 fontPos = new Vector2(256, 128);
                spriteBatch.DrawString(font, score.ToString(), fontPos, Color.GhostWhite, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
                
            }
            spriteBatch.End();


            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
