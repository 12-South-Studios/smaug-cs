using System;
using System.Collections.Generic;

namespace SmaugCS.News;

public class NewsData(int id)
{
  private readonly List<NewsEntryData> _entries = [];

  public int Id { get; private set; } = id;
  public string Header { get; set; }
  public string Name { get; set; }
  public int Level { get; set; }
  public DateTime CreatedOn { get; set; }
  public string CreatedBy { get; set; }
  public bool Active { get; set; }
  public bool Saved { get; set; } = false;

  public IEnumerable<NewsEntryData> Entries => _entries;

  public void AddEntry(NewsEntryData entry)
  {
    if (!_entries.Contains(entry))
      _entries.Add(entry);
  }
}