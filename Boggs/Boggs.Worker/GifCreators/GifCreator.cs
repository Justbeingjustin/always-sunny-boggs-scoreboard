using Boggs.Worker.Models;
using ImageMagick;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace Boggs.Worker.GifCreators
{
    public class GifCreator
    {
        private List<BeerPlayer> _beerPlayers;

        public GifCreator(List<BeerPlayer> beerPlayers)
        {
            _beerPlayers = beerPlayers;
        }

        public string CreateGif()
        {
            _beerPlayers = _beerPlayers.OrderByDescending(x => x.numberOfBeers).ToList();

            for (int i = 3; i < 17; i++)
            {
                PointF playerLocation = new PointF(70f, 100f);
                PointF beerLocation = new PointF(375f, 100f);

                string imageFilePath = ConfigurationManager.AppSettings["cloudsPath"] + "clouds" + i + ".bmp";
                Bitmap bitmap = (Bitmap)Image.FromFile(imageFilePath);//load the image file
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    using (Font arialFont = new Font("Comic Sans MS", 100f, FontStyle.Italic))
                    {
                        graphics.DrawString("Players", arialFont, Brushes.White, new PointF(25f, 30f));
                        graphics.DrawString("#of Beers", arialFont, Brushes.White, new PointF(250f, 30f));
                    }

                    foreach (var beerPlayer in _beerPlayers)
                    {
                        using (Font arialFont = new Font("Comic Sans MS", 50f, FontStyle.Italic))
                        {
                            graphics.DrawString(beerPlayer.name, arialFont, Brushes.White, playerLocation);
                            graphics.DrawString(beerPlayer.numberOfBeers.ToString(), arialFont, Brushes.White, beerLocation);

                            playerLocation.Y = playerLocation.Y + 45;
                            beerLocation.Y = beerLocation.Y + 45;
                        }
                    }
                }

                bitmap.Save(ConfigurationManager.AppSettings["cloudsFinishPath"] + "clouds" + i + ".png", ImageFormat.Png);//save the image file
            }

            using (MagickImageCollection collection = new MagickImageCollection())
            {
                for (int i = 3; i < 17; i++)
                {
                    collection.Add(ConfigurationManager.AppSettings["cloudsFinishPath"] + "clouds" + i + ".png");
                }

                collection.Optimize();

                var guid = Guid.NewGuid();

                // Save gif
                collection.Write(ConfigurationManager.AppSettings["cloudsGifPath"] + guid.ToString() + ".gif");
                return guid.ToString();
            }
        }
    }
}