using Newtonsoft.Json;
using PacketDesignTool.Managers.Supports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PacketDesignTool.Managers.Parents.Packer;

namespace PacketDesignTool.Managers.Parents
{
    public class Packer
    {
        public delegate void OnPackerMemberEventHandler(object sender, Packer packer, int index);

        public string Name;

        public List<string> MemberTypes;
        public List<string> MemberNames;

        [JsonIgnore]
        public OnPackerMemberEventHandler OnAddMember; //vollatile delegate
        [JsonIgnore]
        public OnPackerMemberEventHandler OnRemoveMember; //vollatile delegate

        public string GetViewStringMember(int index)
        {
            if (MemberTypes.Count <= index) return "None";

            string AllText = MemberTypes[index].ToString() + "=" + MemberNames[index].ToString();

            return AllText;
        }

        public bool Remove(string MemberName)
        {
            int _count = -1;

            for(int i = 0; i < MemberNames.Count; ++i)
            {
                if (MemberNames[i] == MemberName)
                    _count = i;
            }

            if (_count == -1)
                return false;

            if (OnRemoveMember != null)
            {
                OnRemoveMember.Invoke(this, this, _count);
                OnRemoveMember = null; //Vollatile Reset
            }

            MemberNames.RemoveAt(_count);
            MemberTypes.RemoveAt(_count);

            return true;
        }

        public bool RemoveAt(int index)
        {
            if (MemberNames.Count <= index) return false;

            return Remove(MemberNames[index]);
        }
    }

    public abstract class PackerManager : ManagerInterface
    {
        public delegate void OnPackerEventhandler(object sender, Packer packer);

        //설정 값
        public string Prefix;

        public PackerManager()
        {
            
        }

        //이벤트 리스트
        public OnPackerEventhandler OnEnterCompeleted;
        public OnPackerEventhandler OnDeleteComnpeleted;

        //PackerList
        public Dictionary<string, Packer> PackerList = new Dictionary<string, Packer>();

        public Packer this[string Key]
        {
            get => PackerList[Key];       // 읽기
        }

        //이벤트 함수
        public abstract void OnEnterList(Packer packer);
        public abstract void OnDeleteList(Packer packer);

        public bool isContainWithType(string Type)
        {
            foreach(Packer packer in PackerList.Values)
            {
                foreach(string key in packer.MemberTypes)
                {
                    string Original = key.Replace("list:", "").Replace("single:", "");

                    if (Original == Type) return true;
                }
            }

            return false;
        }

        public bool RemoveAllTypeMatch(string Type)
        {
            foreach (Packer packer in PackerList.Values)
            {
                List<string> DestroyList = new List<string>();
                foreach (string key in packer.MemberTypes)
                {
                    string Original = key.Replace("list:", "").Replace("single:", "");

                    if (Original == Type)
                    {
                        DestroyList.Add(key);
                    }
                }

                foreach(string key in DestroyList)
                    packer.MemberTypes.Remove(key);
            }

            return false;
        }

        public Packer GetPacker(string Key)
        {
            if (PackerList.TryGetValue(Key, out Packer packer))
                return packer;

            MessageBox.Show("없는 Key값입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return new Packer();
        }

        public bool RemovePackerMember(string Key, string MemberName)
        {
            if (!PackerList.ContainsKey(Key))
                return false;

            if (PackerList[Key].MemberNames.Contains(MemberName))
            {
                if (PackerList[Key].MemberTypes.Count < 2)
                {
                    MessageBox.Show("클래스에 멤버가 없을 경우\n클래스가 있을 필요가 없습니다.", "에러 - 삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                return PackerList[Key].Remove(MemberName);
            }

            return false;
        }

        public bool RemovePackerMemberAt(string Key, int index)
        {
            if (!PackerList.ContainsKey(Key))
                return false;

            if (PackerList[Key].MemberTypes.Count < 2)
            {
                MessageBox.Show("클래스에 멤버가 없을 경우\n클래스가 있을 필요가 없습니다.", "에러 - 삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return PackerList[Key].RemoveAt(index);
        }

        public bool EnterMember(string Key, string MemberType,  string MemberName)
        {
            if (!PackerList.ContainsKey(Key)) return false;

            PackerList[Key].MemberTypes.Add(MemberType);
            PackerList[Key].MemberNames.Add(MemberName);

            return true;
        }

        //이벤트 호출 함수
        public bool EnterList(string Name, string[] MemberTypes, string[] MemberNames)
        {
            if (PackerList.ContainsKey(Name))
            {
                //TODO 실패 이벤트 실행
                MessageBox.Show("이미 있는 클래스 이름입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            Packer packer = new Packer();
            packer.Name = Name;

            if (MemberTypes != null)
                packer.MemberTypes = ToolStaticLibrary.GetListToString(MemberTypes);
            else
                packer.MemberTypes = new List<string>();

            if (MemberNames != null)
                packer.MemberNames = ToolStaticLibrary.GetListToString(MemberNames);
            else
                packer.MemberNames = new List<string>();

            PackerList.Add(Name, packer);

            if (OnEnterCompeleted != null)
                OnEnterCompeleted.Invoke(this, PackerList[Name]);

            OnEnterList(packer);

            return true;
        }
        public bool Remove(string Name)
        {
            if (!PackerList.ContainsKey(Name))
            {
                //TODO 실패 이벤트 실행
                return false;
            }

            Packer packer = PackerList[Name];
            bool succ = PackerList.Remove(Name);

            if (OnDeleteComnpeleted != null && succ == true)
                OnDeleteComnpeleted.Invoke(this, packer);

            OnDeleteList(packer);

            return succ;
        }

        public void OnImport(object sender, string JsonData)
        {
            Dictionary<string, Packer> CopyPackers = JsonConvert.DeserializeObject<Dictionary<string, Packer>>(JsonData);
        
            foreach(var Packer in CopyPackers)
            {
                Packer packer = Packer.Value;
                EnterList(packer.Name, packer.MemberTypes.ToArray(), packer.MemberNames.ToArray());
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
