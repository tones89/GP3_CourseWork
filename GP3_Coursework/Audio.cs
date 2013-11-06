using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace GP3_Coursework
{
    class Audio
    {

        Dictionary<string, Song> dc_SongCollection;
        Dictionary<string, SoundEffect> dc_SoundFxCollection;

        public void addSong(string songName, Song songObject)
        {
         
            dc_SongCollection.Add(songName, songObject);
        }

        public void getSong(string songName)
        {

            Song tempSong;
            if(dc_SongCollection.ContainsKey(songName))
                {
                    tempSong = dc_SongCollection[songName];
                }
        
        }

        public void AddSoundEffect(string effectName,SoundEffect effectObject)
        {
            dc_SoundFxCollection.Add(effectName,effectObject);
        }

  
        /// <summary>
        /// Play the given song
        /// </summary>
        /// <param name="songName"> The name of the song to play</param>
        public void PlaySong(string songName)
        
        {
            if (dc_SongCollection.ContainsKey(songName))
            {
                Song songToPlay = dc_SongCollection[songName];
                MediaPlayer.Play(songToPlay);
            }
            else
                return;
                
        }

        public void PlaySoundEffect(SoundEffect fxName, SoundEffectInstance sfxi_FxName)        
        {

            sfxi_FxName = fxName.CreateInstance();
            sfxi_FxName.Play();
        }

        

    }
}
