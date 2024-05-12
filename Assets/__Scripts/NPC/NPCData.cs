using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New NPC", menuName = "NPC")]

public class NPCData : ScriptableObject
{
    [SerializeField] private string NPCName;
    [SerializeField] private string NPCTalk;
    [SerializeField] private Sprite NPCImage;
    [SerializeField] private NPCType type;

    public string _npcName => NPCName;
    public string _npcTalk => NPCTalk;
    public Sprite _npcImage => NPCImage;
    public NPCType _type => type;
}
/*
 * npc ����
 * ���� ���� (������, ���� ����)
 * ��ȭ ���� (������ ����, ���� ����(����))
 * �Ϲ� - ����Ʈ�� ��
 * 
 * 
 */