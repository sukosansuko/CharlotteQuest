using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Novel
{

    public class EfectComponent : AbstractComponent
    {
        Sprite[] sprites;   // アニメーションに使うスプライト
        int cnt;    // フレームのカウント用
        int spCnt;
        int nowSp;  // 現在のスプライト番号
        SpriteRenderer myRenderer;  // キャッシュ
        [System.NonSerialized] public bool active;
        public EfectComponent()
        {
            /*
             [param]
            name=識別するための名前を指定します
            storage=画像ファイルを指定します
            tag=タグ名を指定できます
            layer=表示させるレイヤを指定します。画面の背面から順に、background,Default,character,message,front が指定できます。デフォルトはDefaultが指定されます
            sort=同一レイヤ内の表示順を整数で指定してください
            offset=何個目のスプライトから始めるか指定します
            x=中心からのx位置を指定します
            y=中心からのy位置を指定します
            z=中心からのz位置を指定します
            scale_x=X方向へのイメージの拡大率を指定します。
            scale_y=Y方向へのイメージの拡大率を指定します。
            scale_z=Z方向へのイメージの拡大率を指定します。
            scale=イメージの拡大率を指定します。つまり2と指定すると大きさが２倍になります
            loop=アニメーションをループさせるかを指定します
             */
            this.arrayVitalParam = new List<string>
            {
                "name",
                "storage",
            };
            this.originalParam = new Dictionary<string, string>
            {
                { "name",""},
                { "storage",""},
                { "tag",""},
                { "layer","Default"},
                { "sort","0"},
                { "imagePath",GameSetting.PATH_IMAGE},
                { "flame", "2" },
                { "offset","0" },
                { "x","0"},
                { "y","0"},
                { "z","-3.2"},
                { "scale",""},
                { "scale_x","1"},
                { "scale_y","1"},
                { "scale_z","1"},
                { "loop","false" },
                { "playAwake","false" },
                { "path","false"}, //trueにすると、pathを補完しない
            };
        }

        void Awake()
        {
            //myRenderer = GetComponent();
            //spCnt = sprites.Length;
            //nowSp = 
        }
        public override void start()
        {

            if (this.param["scale"] != "")
            {

                this.param["scale_x"] = this.param["scale"];
                this.param["scale_y"] = this.param["scale"];
                this.param["scale_z"] = this.param["scale"];

            }
            else
            {
                this.param["scale"] = "1";
            }

            Image image = new Image(this.param);

            this.gameManager.imageManager.addImage(image);
            this.gameManager.nextOrder();

            //this.gameManager.scene.MessageSpeed = 0.02f;
            //this.gameManager.scene.coroutineShowMessage (message);

        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
