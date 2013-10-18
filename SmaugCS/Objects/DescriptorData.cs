﻿using SmaugCS.Enums;


namespace SmaugCS.Objects
{
    public class DescriptorData
    {
        public CharacterInstance Character { get; set; }
        public CharacterInstance Original { get; set; }
        public mccp_data mccp { get; set; }
        public bool can_compress { get; set; }
        public string host { get; set; }
        public int port { get; set; }
        public int descriptor { get; set; }
        public ConnectionTypes ConnectionStatus { get; set; }
        public short idle { get; set; }
        public short lines { get; set; }
        public short scrlen { get; set; }
        public bool fcommand { get; set; }
        public char[] inbuf { get; set; }
        public char[] incomm { get; set; }
        public char[] inlast { get; set; }
        public int repeat { get; set; }
        public string outbuf { get; set; }
        public int outsize { get; set; }
        public int outtop { get; set; }
        public string PageBuffer { get; set; }
        public int PageSize { get; set; }
        public int PageTop { get; set; }
        public string PagePoint { get; set; }
        public char PageCommand { get; set; }
        public char PageColor { get; set; }
        public int newstate { get; set; }
        public char prevcolor { get; set; }
        public int ifd { get; set; }

        public DescriptorData()
        {
            inbuf = new char[Program.MAX_INBUF_SIZE];
            incomm = new char[Program.MAX_INPUT_LENGTH];
            inlast = new char[Program.MAX_INPUT_LENGTH];
        }
    }
}