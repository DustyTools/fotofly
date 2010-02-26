// <copyright file="IsoSpeed.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-02-25</date>
// <summary>IsoSpeed</summary>
namespace Fotofly
{
    using System;
    using System.Text;

    public class IsoSpeed
    {
        private int isoSpeed;

        public IsoSpeed()
        {
        }

        public IsoSpeed(string isoSpeed)
        {
            int iso;

            if (int.TryParse(isoSpeed, out iso))
            {
                this.Numeric = iso;
            }
        }

        public IsoSpeed(int isoSpeed)
        {
            this.Numeric = isoSpeed;
        }

        public int Numeric
        {
            get
            {
                return this.isoSpeed;
            }

            set
            {
                if (value > 0 && value < 100000)
                {
                    this.isoSpeed = value;
                }
                else
                {
                    this.isoSpeed = 0;
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is IsoSpeed)
            {
                if ((obj as IsoSpeed).ToString() == this.ToString())
                {
                    return true;
                }
            }

            return false;
        }
        
        public override string ToString()
        {
            if (this.isoSpeed == 0)
            {
                return string.Empty;
            }
            else
            {
                return "ISO-" + this.isoSpeed;
            }
        }
    }
}