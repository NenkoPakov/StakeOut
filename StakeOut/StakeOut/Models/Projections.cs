﻿using StakeOut.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace StakeOut.Models
{
    /// <summary>
    /// Class for accessing available projections
    /// </summary>
    public class Projections
    {
        /// <summary>
        /// Available projections
        /// </summary>
        private Dictionary<enumProjection, Projection> projections;

        /// <summary>
        /// Available projections
        /// </summary>
        public Projections()
        {
            this.projections = new Dictionary<enumProjection, Projection>();

            this.Init();
        }

        /// <summary>
        /// Get a projection
        /// </summary>
        /// <param name="projection">projection</param>
        /// <returns></returns>
        public Projection this[enumProjection projection]
        {
            get
            {
                return this.projections[projection];
            }
        }

        /// <summary>
        /// Initialize all available projections
        /// </summary>
        private void Init()
        {
            // set ellipsoids!!!
            this.projections.Add(enumProjection.WGS84_GEOGRAPHIC, new Projection("WGS84"));
            this.projections.Add(enumProjection.BGS_SOFIA, new Projection(4680000, 8400000));
            this.projections.Add(enumProjection.BGS_1930_24, new Projection(24, 8500000.0, 0.9999));
            this.projections.Add(enumProjection.BGS_1930_27, new Projection(27, 9500000, 0.9999));
            this.projections.Add(enumProjection.BGS_1950_3_24, new Projection(24, 5500000, 1.0));
            this.projections.Add(enumProjection.BGS_1950_3_27, new Projection(27, 95500000, 1.0));
            this.projections.Add(enumProjection.BGS_1950_6_21, new Projection(21, 4500000, 1.0));
            this.projections.Add(enumProjection.BGS_1950_6_27, new Projection(27, 5500000, 1.0));
            this.projections.Add(enumProjection.BGS_1970_K3, new Projection("k3"));
            this.projections.Add(enumProjection.BGS_1970_K5, new Projection("k5"));
            this.projections.Add(enumProjection.BGS_1970_K7, new Projection("k7"));
            this.projections.Add(enumProjection.BGS_1970_K9, new Projection("k9"));
            this.projections.Add(enumProjection.BGS_2005_KK, new Projection(42.0, 43.333333333333336, 42.667875683333333, 25.5, 0.0, 500000.0, 1.0));
            this.projections.Add(enumProjection.UTM34N, new Projection(21, 500000.0, 0.9996));
            this.projections.Add(enumProjection.UTM35N, new Projection(27, 500000.0, 0.9996));
        }
    }
}
