﻿using System;

namespace SmaugCS.Board;

public class NoteData(int id)
{
    public int ID { get; private set; } = id;
    public string Sender { get; set; }
    public DateTime DateSent { get; set; }
    public string RecipientList { get; set; }
    public string Subject { get; set; }
    public bool IsPoll { get; set; }
    public string YesVotes { get; set; }
    public string NoVotes { get; set; }
    public string Abstentions { get; set; }
    public string Text { get; set; }
    public bool Saved { get; set; }
}