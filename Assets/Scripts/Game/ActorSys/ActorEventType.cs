public enum ActorEventType
{
    #region 上层的控制

    /// <summary>
    /// 驱动AI移动消息
    /// 参数 Vector3 目标位置
    /// ..OnMoveTo
    /// </summary>
    OnCtrlMoveTo,

    /// <summary>
    /// 移动到目标npc
    /// </summary>
    CtrlMoveToNpc,

    /// <summary>
    /// 释放技能
    /// 参数 uint 技能
    /// </summary>
    DoSkill,

    /// <summary>
    /// 自动移动
    /// </summary>
    OnAutoMove,

    /// <summary>
    /// 停止跟随
    /// </summary>
    StopFollow,

    //         /// <summary>
    //         /// 设置宠物配音
    //         /// 参数PetDubbingTriggerType 触发类型
    //         /// </summary>
    //         ChangeDubbingTriggerType,

    /// <summary>
    /// 发送AI切换事件
    /// 参数AIStateEvent 
    /// 参数object AI事件绑定参数
    /// </summary>
    ActorAIChangeEvent,

    /// <summary>
    /// 编辑下自动模式修改
    /// </summary>
    SkillEditorAutoModeChagne,


    #endregion

    #region 移动控制相关

    /// <summary>
    /// 开始飞行
    /// </summary>
    StartFlyEvent,

    /// <summary>
    /// 停止飞行
    /// </summary>
    StopFlyEvent,

    /// <summary>
    /// 开始移动
    /// 参数 Vector3 目标位置
    /// </summary>
    StartMoveEvent,

    /// <summary>
    /// 移动到距离停止
    /// 参数vector3 目标位置
    /// 参数float 停止距离
    /// </summary>
    StartMoveDistEvent,

    /// <summary>
    /// 移动控制
    /// </summary>
    StopMoveEvent,

    /// <summary>
    /// 移动行为或者数据变化了
    /// 参数 bool  是否移动了
    /// </summary>
    ActorMoveChangeEvent,

    /// <summary>
    /// 暂停移动
    /// </summary>
    LeaveMoveStateEvent,

    /// <summary>
    /// 移动到目标对象位置
    /// </summary>
    StartToTarget,

    /// <summary>
    /// 到达目标事件
    /// </summary>
    ArriveToTarget,

    #endregion

    #region 联网移动相关

    /// <summary>
    /// 服务器通知更新当前位置
    /// </summary>
    SVR_MOVE_SET_CURR_POS,

    /// <summary>
    /// 服务器通知开始移动
    /// </summary>
    SVR_MOVE_START_MOVE,

    #endregion

    #region 状态相关

    /// <summary>
    /// 状态变化
    /// 参数ActorStateEvent 变化的event类型
    /// </summary>
    ActorStateChange,

    /// <summary>
    /// Buff状态变化
    /// </summary>
    ActorBuffStateChange,

    /// <summary>
    /// Buff变化
    /// </summary>
    ActorBuffChange,

    #endregion

    #region 技能相关
    SkillImpacted,

    /// <summary>
    /// 受击动画播放结束
    /// </summary>
    AnimImpactEnd,

    /// <summary>
    /// 被技能推动的消息
    /// 参数 SkillTransUnit 位移数据
    /// </summary>
    StartTranslateEvent,

    /// <summary>
    /// 结束推动的消息
    /// </summary>
    ClearTranslateEvent,

    #endregion

    #region AI相关

    /// <summary>
    /// 被攻击消息
    /// </summary>
    ON_ATTACKED_EVENT,

    /// <summary>
    /// 被攻击消息经常收不到消息，改为收到伤害事件
    /// </summary>
    ON_DAMGEED_EVENT,

    /// <summary>
    /// 修改AI
    /// </summary>
    ChangeActorAi,

    #endregion

    #region OnlineEvent
    /// <summary>
    /// 开始播放技能
    /// </summary>
    /// <param type="SkillCmdInfo">技能的相关信息</param>
    SKILL_START_CAST,

    /// <summary>
    /// 取消播放技能
    /// </summary>
    /// <param type="uint">技能GID</param>
    SKILL_CANCE_CAST,

    /// <summary>
    /// 通知技能的伤害
    /// </summary>
    /// <param type="CSMapActorDamageData">技能伤害信息</param>
    SKILL_SYNC_DAMAGE,

    /// <summary>
    /// 同步玩家基础数据
    /// </summary>
    SKILL_SYNC_BASE_INFO,


    /// <summary>
    /// 重新同步位置等坐标信息
    /// </summary>
    SKILL_SYNC_POSITION,


    /// <summary>
    /// 同步Buff信息
    /// </summary>
    /// <param type="BuffSetInfo">最终的Buff信息</param>
    SKILL_SYNC_BUFF,

    /// <summary>
    /// 同步buff导致的死亡事件
    /// <param type="BuffSetInfo">最终的Buff信息</param>
    /// </summary>
    SKILL_SYNC_BUFF_KILL,

    /// <summary>
    /// 同步buff动态运行数据
    /// </summary>
    SKILL_SYNC_BUFF_RUNDATA,

    /// <summary>
    /// 强制同步buff动态运行数据
    /// </summary>
    SKILL_SYNC_BUFF_RUNDATA_FORCE,

    /// <summary>
    /// 同步锁定目标事件
    /// </summary>
    SKILL_SYNC_LOCK_TARGET,

    /// <summary>
    /// 掉落箱子消失
    /// </summary>
    DROP_BOX_DISAPPEAR,

    #endregion

    #region 模型相关

    /// <summary>
    /// 模型销毁之前
    /// </summary>
    BeforeModelDestroy,

    /// <summary>
    /// 模型销毁之后
    /// </summary>
    AfterModelDestroy,

    /// <summary>
    /// 模型创建之前
    /// </summary>
    BeforeModelCreated,

    /// <summary>
    /// 模型创建之后
    /// </summary>
    AfterModelCreated,

    /// <summary>
    /// 模型显示变化
    /// </summary>
    ModelVisibleChange,

    /// <summary>
    /// 模型动画变化导致
    /// 参数 ModelType
    /// </summary>
    ModelAnimatorChange,

    //         /// <summary>
    //         /// 主模型显示和隐藏
    //         /// 参数bool
    //         /// </summary>
    //         MainModelVisibleChange,

    /// <summary>
    /// 重新刷新下模型的渲染
    /// 参数ShowLightProbe 环境光照
    /// 参数MainActorLight 主光源
    /// </summary>
    RefreshRender,


    /// <summary>
    /// 当前相机变化通知
    /// Camera 相机
    /// Transform 相机的Transform
    /// </summary>
    OnCameraChanged,
    #endregion

    #region 一些交互相关

    /// <summary>
    /// 开始交互
    /// 参数ActorInteractionData
    /// </summary>
    StartInteraction,

    /// <summary>
    /// 重新刷新显示
    /// </summary>
    ResetDisplayInfo,

    /// <summary>
    /// 跟随主人移动
    /// </summary>
    FollowMasterMoveEvent,

    /// <summary>
    /// 宠物切换台词
    /// 参数PetWordTriggerType 触发类型
    /// </summary>
    ChangeWordTriggerType,

    /// <summary>
    /// Actor被选中
    /// </summary>
    ActorBeChosed,

    /// <summary>
    /// Actor被取消选中
    /// </summary>
    ActorBeUnChosed,

    /// <summary>
    /// Actor被攻击
    /// </summary>
    ActorBeAttacked,

    /// <summary>
    /// Actor名字改变
    /// </summary>
    ActorNameChange,

    /// <summary>
    /// Actor称号改变
    /// </summary>
    ActorChengHaoChange,

    #endregion
}