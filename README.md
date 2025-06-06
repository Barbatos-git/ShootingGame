シューティングゲーム

このゲームは、Unityエンジンで制作された2Dシューティングゲームです。
リアルタイムの攻撃とカード戦略を組み合わせたゲームで、プレイヤーは敵の弾幕を回避しながら敵を撃破し、最終的にボスとのカードバトルに挑みます。

【ゲーム概要】
- プレイヤーは移動と通常弾での攻撃が可能です。
- 敵を10体倒すごとに1枚カードを獲得します（最大3枚所持）。
- ボスは初期から3枚のカードを持ち、プレイヤーと「3枚カード」ルールで対戦します。
- プレイヤーは５秒ごとに新しいカードを獲得。
- 敵を倒すことでスコアと撃破数を蓄積、ボス撃破でボーナス加点。

【カードシステム】
- 発牌：撃破数に応じて自動でカードを取得、プレイヤーとボスは初期3枚。
- 備えカード：手札が満杯のときは予備エリアに送られ、５秒後に消滅。
- 出牌操作：J/K/L キーでカードを選び、ボスと対決（スート無視の強さ比較）。
- 弾種連動：出したカードによって通常弾・竜巻・落雷の特殊弾が発動。

【UIと演出】
- 黒幕フェードイン・フェードアウトによるシーン切り替え。
- プレイヤーやボス死亡時に爆発演出後、エンド演出に遷移。
- EndSceneで撃破数・スコア・プレイ時間・ランク評価（SSS〜D）を表示。
- Sランク以上で光るテキスト演出あり。
- ESCキーでいつでもゲーム終了。

【ディレクトリ構成】
- Assets/
  - Scripts          : ゲームロジック（プレイヤー、ボス、カード）
  - Prefabs          : プレイヤー、敵、弾、カードのプレハブ
  - Scenes           : TitleScene / GameScene / EndScene
  - Animation        : Animator Controller とアニメーションファイル
  - AudioSource      : サウンド（銃声、風、雷など）
  - Resource         : カード画像やアニメーション素材

【操作方法】
矢印キー または WASDキー  : プレイヤー移動  
スペースキー             : ゲーム開始 / シーン遷移 / 弾の発射
J / K / L               : カード使用（3つの手札に対応）  
ESC                     : ゲーム終了 

【使用技術・プラグイン】
- Unity 6.0
- DOTween（演出・アニメーション管理）
- TextMeshPro（テキストUI）
- 独自実装のカードシステムおよび弾幕処理ロジック

【特徴まとめ】
- 弾幕×カードバトルという新感覚の融合システム
- シーン遷移や演出を丁寧に作り込んだ高品質UI
- タイトル → 戦闘 → エンディングまで一貫した構成
- モジュール化されたスクリプトで拡張性も高い
