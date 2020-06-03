﻿// Copyright (c) 2020 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

namespace Test.Chapter08Listings.EfClasses
{
    public enum TrackNames { NetCore = 0, EfCore = 1, AspNetCore = 3}

    public class OptionalTrack
    {
        public int OptionalTrackId { get; set; }

        public TrackNames Track { get; set; }

        public Attendee Attend { get; set; }
    }
}