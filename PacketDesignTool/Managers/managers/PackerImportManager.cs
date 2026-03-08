using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketDesignTool.Managers
{
    public class PackerImportManager : ManagerInterface
    {
        public delegate void OnPackerImportEventHandler(object sender, string[] settingDatas);
        public delegate void OnPackerImportPackerHandler(object sender, string JsonData);

        private static PackerImportManager _Inst;
        public static PackerImportManager Inst => _Inst ?? (_Inst = new PackerImportManager());

        //외부 이벤트 핸들러
        public OnPackerImportEventHandler onPackerImportEventHandler;
        public OnPackerImportPackerHandler onPackerClassImportHandler;
        public OnPackerImportPackerHandler onPackerStructImportHandler;
        public OnPackerImportPackerHandler onSettingsImportHandler;
        public OnPackerImportPackerHandler onTagsImportHandler;
        public OnPackerImportPackerHandler onEventPacketImportHandler;

        public void Import(string Path)
        {
            if (System.IO.File.Exists(Path)) 
            {
                string AllText = System.IO.File.ReadAllText(Path);

                string[] Sliced = AllText.Split(new string[] { "[EndLine/]" }, StringSplitOptions.None);

                for (int i = 0; i < Sliced.Length; ++i)
                {
                    string[] Headers = new string[2];

                    for(int j = 0; j < Sliced[i].Length; ++j)
                    {
                        if(Sliced[i][j] != ']')
                        {
                            if (Sliced[i][j] != '\r' && Sliced[i][j] != '\n')
                                Headers[0] += Sliced[i][j];
                        }
                        else
                        {
                            Headers[0] += Sliced[i][j];
                            break;
                        }
                    }

                    for (int j = Sliced[i].Length - 1; j >= 0; --j)
                    {
                        if (Sliced[i][j] != '[')
                        {
                            if (Sliced[i][j] != '\r' && Sliced[i][j] != '\n')
                                Headers[1] += Sliced[i][j];
                        }
                        else
                        {

                            Headers[1] += Sliced[i][j];
                            char[] reverse = Headers[1].ToCharArray();
                            Array.Reverse(reverse);
                            Headers[1] = new string(reverse);

                            break;
                        }
                    }

                    Sliced[i] = Sliced[i].Replace(Headers[0], "").Replace(Headers[1], "");
                    
                    if (Headers[0].Contains(ClassPackerManager.Inst.Prefix)) //클래스 패커
                    {
                        onPackerClassImportHandler.Invoke(this, Sliced[i]);
                    }
                    else if (Headers[0].Contains(StructPackerManager.Inst.Prefix)) //구조체 패커
                    {
                        onPackerStructImportHandler.Invoke(this, Sliced[i]);
                    }
                    else if (Headers[0].Contains("Settings"))
                    {
                        onSettingsImportHandler.Invoke(this, Sliced[i]);
                    }
                    else if (Headers[0].Contains("Tags"))
                    {
                        onTagsImportHandler.Invoke(this, Sliced[i]);
                    }
                    else if (Headers[0].Contains("EventPackets"))
                    {
                        onEventPacketImportHandler.Invoke(this, Sliced[i]);
                    }
                }

                if (onPackerImportEventHandler != null)
                    onPackerImportEventHandler.Invoke(null, Sliced);
            }
        }

        public void OnEnable(object sender, object EventKeyword)
        {

        }

        public void OnDisable(object sender, object EventKeyword)
        {

        }
    }
}
