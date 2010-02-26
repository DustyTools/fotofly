// <copyright file="Rating.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-02-25</date>
// <summary>Rating</summary>
namespace Fotofly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml.Serialization;

    public class Rating : ICloneable
    {
        public enum Ratings
        {
            NoRating,
            Rejected,
            OneStar,
            TwoStar,
            ThreeStar,
            FourStar,
            FiveStar
        }

        private readonly double maxValue = 5.0;
        private readonly double minValue = -1.0;
        private double rating;

        public Rating()
        {
            this.rating = 0;
        }

        public Rating(double rating)
        {
            this.Numerical = rating;
        }

        public Rating(string rating)
        {
            double ratingDouble;

            if (double.TryParse(rating, out ratingDouble))
            {
                this.Numerical = ratingDouble;
            }
            else
            {
                this.Numerical = 0;
            }
        }

        public double Numerical
        {
            get
            {
                return this.rating;
            }

            set
            {
                if (value > maxValue)
                {
                    this.rating = maxValue;
                }
                else if (value < minValue)
                {
                    this.rating = minValue;
                }
                else
                {
                    this.rating = value;
                }
            }
        }

        public Ratings AsEnum
        {
            get
            {
                switch((int)Math.Round(this.rating, 0))
                {
                    case -1:
                        return Ratings.Rejected;

                    default:
                    case 0:
                        return Ratings.NoRating;

                    case 1:
                        return Ratings.OneStar;

                    case 2:
                        return Ratings.TwoStar;

                    case 3:
                        return Ratings.ThreeStar;

                    case 4:
                        return Ratings.FourStar;

                    case 5:
                        return Ratings.FiveStar;
                }
            }
        }

        public object Clone()
        {
            return new Rating(this.rating);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
