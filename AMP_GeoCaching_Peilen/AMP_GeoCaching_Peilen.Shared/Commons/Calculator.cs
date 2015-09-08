﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AMP.GeoCachingTools.Commons
{

    class Calculator
    {

        public double longitude { get; set; }

        public double latitude { get; set; }

        private double distance;

        private double direction;

        public Calculator(double longitude, double latitude, double distance, double direction)
        {
            this.longitude = longitude;
            this.latitude = latitude;
            this.distance = distance;
            this.direction = direction;
        }

        public Calculator()
        {
            this.longitude = 0;
            this.latitude = 0;
            this.distance = 0;
            this.direction = 0;
        }

        // The main calculation
        public void calculate ()
        {
            System.Diagnostics.Debug.WriteLine("Ausgangsbreitengrad : " + latitude.ToString());
            System.Diagnostics.Debug.WriteLine("Ausgangslängengrad : " + longitude.ToString());

            double deltaLongitude = 0;
            // Distance comes in metres. We need it in kilometres.
            distance = distance / 1000;

            // Trigonometry : deltaLongitude = (cos(direction) * distance)
            // For longitudes : 1° is round about 111,12 km
            deltaLongitude = ((Math.Cos(direction) * distance) / 75); 

            // Trigonometry : deltaLatitude = (sin(direction) * distance)
            // For latitudes : 1° is round about 73,62 km
            latitude = ((Math.Sin(direction) * distance) / (75 * Math.Cos(longitude))) + latitude;

            longitude += deltaLongitude;

            System.Diagnostics.Debug.WriteLine("Ausgangsbreitengrad : " + latitude.ToString());
            System.Diagnostics.Debug.WriteLine("Ausgangslängengrad : " + longitude.ToString());
        }

        // ############################
        // ## Methods for Converting ##
        // ############################

        // Convert from '50 25.123' to '50.4187166666'
        public double convertDegreesMinutesToDegrees(string coordinate)
        {
            double degrees;
            double arcminutes;
            string[] finalValues = new string[2];
            string[] values = coordinate.Split(' ');
            int length = values.Length;

            for (int count = 0; count < length; count++)
            {
                finalValues[count] = values[count];
            }

            if (length < finalValues.Length)
            {
                finalValues[1] = "0";
            }

            // fill with zeros
            if (finalValues[1].Length < 6)
            {
                finalValues[1] = fillWithZeros(finalValues[1]);
            }

            Double.TryParse(finalValues[0], out degrees);
            Double.TryParse(finalValues[1], out arcminutes);

            // Arcminutes / 1000  = solution for decimal and (solution / 60) for decimal degrees
            arcminutes = arcminutes / 1000 / 60;
            // We have to add the decimal degrees from arcminutes to the integer of degrees
            degrees += arcminutes;

            return degrees;
        }

        // Convert from '10.102' to '10,102'
        public double convertDegreesStringToDegrees(string coordinate)
        {
            double degrees;
            Double.TryParse(coordinate.Replace(".", ","), out degrees);
            return degrees;
        }

        // Convert from '52 31 14.941' to '50.4187166666'
        public double convertDegreesMinutesSecondsToDegrees(string coordinate)
        {
            double degrees;
            double arcminutes;
            double arcseconds;
            string[] finalValues = new string[3];
            string[] values = coordinate.Split(' ');
            int length = values.Length;

            for (int count = 0; count < length; count++)
            {
                finalValues[count] = values[count];
            }

            if (length == 1)
            {
                finalValues[1] = "0";
            }
            else if (length == 2)
            {
                finalValues[2] = "0";
            }

            // fill with zeros
            if (finalValues[1].Length < 6)
            {
                finalValues[1] = fillWithZeros(finalValues[1]);
            }

            // fill with zeros
            if (finalValues[2].Length < 6)
            {
                finalValues[2] = fillWithZeros(finalValues[2]);
            }

            Double.TryParse(finalValues[0], out degrees);
            Double.TryParse(finalValues[1], out arcminutes);
            Double.TryParse(finalValues[2], out arcseconds);

            // Arcseconds / 1000  = solution for decimal and (solution / 60) for arcminutes
            arcseconds = arcseconds / 1000 / 60;
            // Arcminutes / 1000  = solution for decimal and (solution / 60) for decimal degrees and add the arcseconds
            arcminutes = (arcminutes / 1000 / 60) + arcseconds;
            // We have to add the decimal degrees from arcminutes to the integer of degrees
            degrees = degrees + arcminutes;

            return degrees;
        }

        // Convert from '50.4187166666' to '50 25.123'
        public string convertDegreesToDegreesMinutes(double coordinate)
        {
            double degrees = Math.Truncate(coordinate);
            double arcminutes = Math.Round((coordinate - degrees) * 60, 3);

            return degrees.ToString() + " " + arcminutes.ToString().Replace(",", ".");
        }

        // Convert from '10,102' to '10.102'
        public string convertDegreesToDegreesString(double coordinate)
        {
            return Math.Round(coordinate, 6).ToString().Replace(",", ".");
        }

        // Convert from '50.4187166666' to '50 25.123'
        public string convertDegreesToDegreesMinutesSeconds(double coordinate)
        {
            double degrees = Math.Truncate(coordinate);
            double arcminutesDecimal = coordinate - degrees;
            double arcminutes = Math.Truncate(arcminutesDecimal);
            double arcseconds = Math.Round((arcminutesDecimal - arcminutes) * 60, 3);

            return degrees.ToString() + " " + arcminutes.ToString().Replace(",", ".") + " " + arcseconds.ToString().Replace(",", ".");
        }

        // if a arcminute or arcsecond has not six elements, the conversation is going to be failed
        private string fillWithZeros(string value)
        {
            return value.PadRight(6, '0');
        }

    }
}
