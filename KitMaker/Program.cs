using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitMaker
{
    class KitMaker
    {
        static void Main(string[] args)
        {
            KitMaker km = new KitMaker(9);

            using (StreamWriter sw = new StreamWriter("./kit.css")) { 
                km.WriteCss(sw);
            }

            using (StreamWriter sw = new StreamWriter("./kit.html"))
            {
                km.WriteHTML(sw);
            }
        }

        private int BlockCount { get; set; }
        private int AnimationSeconds { get; set; } = 8;
        private double LitTime = 1;
        private double FadeInTime = .25;
        private double FadeOutTime = .5;
        private string Height = "6px";

        private string DarkColor = "00274c";
        private string LightColor = "ffcb05";
        private string BorderColor = "eee";
        private KitMaker(int blocks)
        {
            BlockCount = blocks;

        }


        private void WriteCss(StreamWriter writer)
        {
            writer.WriteLine(".kit {");
            writer.WriteLine("  width: 100%;");
            writer.WriteLine($" height: {Height};");
            writer.WriteLine($"  background-color:#{DarkColor};");
            writer.WriteLine("}");
            writer.WriteLine("");

            writer.WriteLine(".kit .block {");
            writer.WriteLine("  float: left;");
            writer.WriteLine($"  width:{(100.00/BlockCount).ToString("0.####")}%;");
            writer.WriteLine("  height:100%;");
            writer.WriteLine("  padding:1px;");
            writer.WriteLine("  border-style:solid;");
            writer.WriteLine($"  border-color: #{BorderColor};");
            writer.WriteLine("  border-width:1px;");
            writer.WriteLine("}");
            writer.WriteLine(""); writer.WriteLine(""); writer.WriteLine("");

            Dictionary<int, double[]> eventToKeyframes = new Dictionary<int, double[]>();

            eventToKeyframes[0] = new double[4];
            eventToKeyframes[0][0] = 0;
            eventToKeyframes[0][1] = eventToKeyframes[0][0] + FadeInTime;
            eventToKeyframes[0][2] = eventToKeyframes[0][1] + LitTime;
            eventToKeyframes[0][3] = eventToKeyframes[0][2] + FadeOutTime;

            System.Diagnostics.Debug.WriteLine($"Event {1}: {eventToKeyframes[0][0]}\t{eventToKeyframes[0][1]}\t{eventToKeyframes[0][2]}\t{eventToKeyframes[0][3]}");

            for (int lightEvent = 1; lightEvent <= BlockCount  * 2 - 1; ++lightEvent)
            {
                eventToKeyframes[lightEvent] = new double[4];
                eventToKeyframes[lightEvent][0] = eventToKeyframes[lightEvent - 1][2] - FadeInTime;
                eventToKeyframes[lightEvent][1] = eventToKeyframes[lightEvent][0] + FadeInTime;
                eventToKeyframes[lightEvent][2] = eventToKeyframes[lightEvent][1] + LitTime;
                eventToKeyframes[lightEvent][3] = eventToKeyframes[lightEvent][2] + FadeOutTime;
                System.Diagnostics.Debug.WriteLine($"Event {lightEvent}: {eventToKeyframes[lightEvent][0]}\t{eventToKeyframes[lightEvent][1]}\t{eventToKeyframes[lightEvent][2]}\t{eventToKeyframes[lightEvent][3]}");
            }


            double totalTime = eventToKeyframes[BlockCount*2-1][3];



            for( int i=1; i<=BlockCount; ++i)
            {
                writer.WriteLine($".kit .block.block{i} {{");
                writer.WriteLine($" -webkit-animation: animate-kitblock{i} {AnimationSeconds}s infinite ease-in-out;");
                writer.WriteLine($" animation: animate-kitblock{i} {AnimationSeconds}s infinite ease-in-out;");
                writer.WriteLine("}");
                writer.WriteLine("");

                if (i == BlockCount) {
                    string keyframe1 = (eventToKeyframes[i - 1][0] * 100 / totalTime).ToString("0.###");
                    string keyframe2 = (eventToKeyframes[i - 1][1] * 100 / totalTime).ToString("0.###");
                    string keyframe3 = (eventToKeyframes[i - 1][2] * 100 / totalTime).ToString("0.###");
                    string keyframe4 = (eventToKeyframes[i - 1][3] * 100 / totalTime).ToString("0.###");
                    writer.WriteLine($"@-webkit-keyframes animate-kitblock{i} {{");
                    writer.WriteLine($"  0%, {keyframe1}% {{");
                    writer.WriteLine($"    background-color:#{DarkColor};");
                    writer.WriteLine($"  }}");
                    writer.WriteLine($"  {keyframe2}%, {keyframe3}% {{");
                    writer.WriteLine($"    background-color:#{LightColor};");
                    writer.WriteLine($"  }}");
                    writer.WriteLine($"  {keyframe4}% {{");
                    writer.WriteLine($"    background-color:#{DarkColor};");
                    writer.WriteLine($"  }}");
                    writer.WriteLine($"}}");

                } else {
                    int inEvent = i;
                    int outEvent = ((BlockCount * 2) - inEvent);
                    string keyframe1 = (eventToKeyframes[inEvent-1][0] * 100 / totalTime).ToString("0.###");
                    string keyframe2 = (eventToKeyframes[inEvent - 1][1] * 100 / totalTime).ToString("0.###");
                    string keyframe3 = (eventToKeyframes[inEvent - 1][2] * 100 / totalTime).ToString("0.###");
                    string keyframe4 = (eventToKeyframes[inEvent - 1][3] * 100 / totalTime).ToString("0.###");
                                       
                    string keyframe5 = (eventToKeyframes[outEvent - 1][0] * 100 / totalTime).ToString("0.###");
                    string keyframe6 = (eventToKeyframes[outEvent - 1][1] * 100 / totalTime).ToString("0.###");
                    string keyframe7 = (eventToKeyframes[outEvent - 1][2] * 100 / totalTime).ToString("0.###");
                    string keyframe8 = (eventToKeyframes[outEvent - 1][3] * 100 / totalTime).ToString("0.###");


                    writer.WriteLine($"@-webkit-keyframes animate-kitblock{i}{{");
                    writer.WriteLine($"  0%, {keyframe1}% {{");
                    writer.WriteLine($"      background-color:#{DarkColor};");
                    writer.WriteLine($"  }} {keyframe2}%, {keyframe3}% {{");
                    writer.WriteLine($"      background-color:#{LightColor};");
                    writer.WriteLine($"  }} {keyframe4}%, {keyframe5}% {{");
                    writer.WriteLine($"      background-color:#{DarkColor};");
                    writer.WriteLine($"  }} {keyframe6}%, {keyframe7}% {{");
                    writer.WriteLine($"      background-color:#{LightColor};");
                    writer.WriteLine($"  }} {keyframe8}% {{");
                    writer.WriteLine($"      background-color:#{DarkColor};");
                    writer.WriteLine($"  }}");
                    writer.WriteLine("}");
                }

            }
        }

        private void WriteHTML(StreamWriter writer)
        {
            writer.WriteLine("<div class='kit'>");
            for (int i = 1; i <= BlockCount; ++i)
            {
                writer.WriteLine($" <div class='block block{i}'></div>");
            }
            writer.WriteLine("</div>");
        }
    }
}
