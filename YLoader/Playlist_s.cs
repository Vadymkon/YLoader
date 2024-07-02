using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YLoader
{
    class Playlist_s
    {
        public List<Playlist> playlists;
        public Playlist_s(List<Playlist> pl)
        {
            playlists = pl;
            final_sort();
        }

        public int Count() { return playlists.Count; }

        public void bucnut_na(int step = 2, int index = 0)
        {
            //if (index > Count()) return; //safety

            //moving
            Playlist buffer = playlists[index]; 
            playlists.RemoveAt(index);
            if (index + step < playlists.Count) //if safety rangeout
                playlists.Insert(index + step, buffer); // move to 2 step right
            else
                playlists.Insert(index + step - playlists.Count, buffer); //else move to "start of list"
        }

        public void random_sort()
        {
            playlists.Shuffle();
            no_pairs();
            no_pairs();
            no_pairs();
        }

        public void no_pairs()
        {
            for (int i = 0; i < playlists.Count - 1; i++) //for pairs in playlists
            {
                if (playlists[i].type.Length == 1 && playlists[i + 1].type.Length == 1) //if there's not ""
                    if (playlists[i].type == playlists[i + 1].type) //MM or PP
                        bucnut_na(index: i + 1); //moving
            }
            final_sort();
        }

        public void final_sort()
        {
            List<Playlist> buffer = playlists.Where(x => x.NameOfPlaylist == "somn").ToList();
            if (buffer.Count > 0)
            {
                playlists.Remove(buffer[0]);
                playlists.Insert(0, buffer[0]);
            }
            buffer = playlists.Where(x => x.NameOfPlaylist == "final").ToList();
            if (buffer.Count > 0)
            {
                playlists.Remove(buffer[0]);
                playlists.Add(buffer[0]);
            }
        }
        
    }
}
