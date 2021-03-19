using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

#region TalentConfig
[XmlRoot("TalentConfig")]
[Cfg("Assets/VGBasic/ConfigTools/TalentConfig.xml")]
public class TalentCfg
{
    [XmlArray("RoleTalent"), XmlArrayItem("Talent")]
    public List<Talent> talents = new List<Talent>();
    [XmlArray("Random"), XmlArrayItem("RandomSkill")]
    public List<RandomSkill> randomSkills = new List<RandomSkill>();

    void Init()
    {

    }
}

public class Talent
{
    [XmlAttribute("Kind")]
    public int kind;
    [XmlAttribute("SkillOpenGrade")]
    public string SkillOpenGrade;
    [XmlAttribute("SkillSwitch")]
    public string SkillSwitch;
    [XmlAttribute("IsFixedSkill")]
    public string IsFixedSkill;                 //1 角色固定技能   随机技能花费
    [XmlAttribute("FixedSkillID")]
    public string FixedSkillID;
    [XmlAttribute("skillName")]
    public string skillName;
}

public class RandomSkill
{
    [XmlAttribute("roleid")]
    public int roleid;
    [XmlAttribute("kind")]
    public int kind;
    [XmlAttribute("name")]
    public string name;
    [XmlAttribute("unlocked")]
    public int unlocked;
    [XmlAttribute("skillType")]
    public int skillType;
    [XmlAttribute("intrduce")]
    public string intrduce;
    [XmlAttribute("grade")]
    public string grade;
}
#endregion

