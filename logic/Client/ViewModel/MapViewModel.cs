﻿using Client.Model;
using Protobuf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Dispatching;
using Grpc.Core;
using Client.Util;


namespace Client.ViewModel
{
    public partial class GeneralViewModel : BindableObject
    {

        private List<MessageOfAll> listOfAll;
        private List<MessageOfSweeper> listOfSweeper;
        private List<MessageOfBullet> listOfBullet;
        private List<MessageOfBombedBullet> listOfBombedBullet;
        private List<MessageOfRecycleBank> listOfRecycleBank;
        private List<MessageOfSignalTower> listOfSignalTower;
        private List<MessageOfChargeStation> listOfChargeStation;
        private List<MessageOfBridge> listOfBridge;
        private List<MessageOfGarbage> listOfGarbage;
        private List<MessageOfHome> listOfHome;

        /* initiate the Lists of Objects and CountList */
        private void InitiateObjects()
        {
            listOfAll = new List<MessageOfAll>();
            listOfSweeper = new List<MessageOfSweeper>(); ;
            listOfBullet = new List<MessageOfBullet>();
            listOfBombedBullet = new List<MessageOfBombedBullet>();
            listOfRecycleBank = new List<MessageOfRecycleBank>();
            listOfChargeStation = new List<MessageOfChargeStation>();
            listOfSignalTower = new List<MessageOfSignalTower>();
            listOfGarbage = new List<MessageOfGarbage>();
            listOfHome = new List<MessageOfHome>();
            listOfBridge = new List<MessageOfBridge>();
            countMap = new Dictionary<int, int>();
        }
        private (int x, int y)[] resourcePositionIndex;
        private (int x, int y)[] RecycleBankPositionIndex;
        private (int x, int y)[] ChargeStationPositionIndex;
        private (int x, int y)[] SignalTowerPositionIndex;
        private (int x, int y)[] wormHolePositionIndex;
        private Dictionary<int, int> countMap;


        private int[,] defaultMap;
        ///* Get the Map to default map */
        private void GetMap(MessageOfMap obj)
        {
            int[,] map = new int[50, 50];
            try
            {
                for (int i = 0; i < 50; i++)
                {
                    for (int j = 0; j < 50; j++)
                    {
                        map[i, j] = Convert.ToInt32(obj.Rows[i].Cols[j]) + 4; // 与proto一致
                    }
                }
            }
            catch
            {
                getMapFlag = false;
            }
            finally
            {
                defaultMap = map;
                getMapFlag = true;
            }
        }

        //public class XY
        //{
        //    volatile int x = 10;
        //    volatile int y = 10;
        //    public int X 
        //    {
        //        get => Interlocked.CompareExchange(ref x,-1,-1); 
        //        set => Interlocked.Exchange(ref x, value); 
        //    }
        //    public int Y 
        //    {
        //        get => Interlocked.CompareExchange(ref y,-1,-1); 
        //        set => Interlocked.Exchange(ref y, value);
        //    } 
        //    public void AddX(int value) => Interlocked.Add(ref x, value);
        //    public void AddY(int value) => Interlocked.Add(ref y, value);
        //}

        //private XY ballxy = new XY();

        //public void Draw(ICanvas canvas, RectF dirtyRect)
        //{
        //    lock (drawPicLock)
        //    {
        //        System.Diagnostics.Debug.WriteLine(String.Format("Draw--cou:{0}, coud{1}", cou, Countdow));

        //        System.Diagnostics.Debug.WriteLine("Draw");
        //        canvas.FillColor = Colors.Red;

        //        // 绘制小球
        //        ballx = ballx_receive;
        //        bally = bally_receive;
        //        canvas.FillEllipse(ballx, bally, 20, 20);
        //        System.Diagnostics.Debug.WriteLine(String.Format("============= Draw: ballX:{0}, ballY:{1} ================", ballx, bally));
        //        System.Diagnostics.Debug.WriteLine(String.Format("============= Draw Receive: ballX:{0}, ballY:{1} ================", ballx_receive, bally_receive));

        //        DrawBullet(new MessageOfBullet
        //        {
        //            X = 10,
        //            Y = 10,
        //            Type = BulletType.NullBulletType,
        //            BombRange = 5
        //        }, canvas);

        //        DrawSweeper(new MessageOfSweeper
        //        {
        //            X = 10,
        //            Y = 11,
        //            Hp = 100,
        //            TeamId = 0
        //        }, canvas);

        //        DrawBullet(new MessageOfBullet
        //        {
        //            X = 9,
        //            Y = 11,
        //            Type = BulletType.NullBulletType,
        //            BombRange = 5
        //        }, canvas);

        //        DrawSweeper(new MessageOfSweeper
        //        {
        //            X = 10,
        //            Y = 12,
        //            Hp = 100,
        //            TeamId = 1
        //        }, canvas);

        //        listOfBullet.Add(new MessageOfBullet
        //        {
        //            X = 20,
        //            Y = 20,
        //            Type = BulletType.NullBulletType,
        //            BombRange = 5
        //        });

        //        listOfSweeper.Add(new MessageOfSweeper
        //        {
        //            X = 10,
        //            Y = 12,
        //            Hp = 100,
        //            TeamId = 1
        //        });

        //        if (listOfBullet.Count > 0)
        //        {
        //            foreach (var data in listOfBullet)
        //            {
        //                DrawBullet(data, canvas);
        //            }
        //        }

        //        if (listOfBullet.Count > 0)
        //        {
        //            foreach (var data in listOfSweeper)
        //            {
        //                DrawSweeper(data, canvas);
        //            }
        //        }
        //    }
        //}

        private Dictionary<MapPatchType, Color> PatchColorDict = new Dictionary<MapPatchType, Color>
        {
            {MapPatchType.RedHome, Colors.Red},
            {MapPatchType.BlueHome, Colors.Blue},
            {MapPatchType.Ruin, Colors.Black},
            {MapPatchType.Grass, Colors.Gray},
            {MapPatchType.River, Colors.Brown},
            {MapPatchType.Garbage, Colors.Yellow},
            {MapPatchType.RecycleBank, Colors.Orange},
            {MapPatchType.ChargeStation, Colors.Chocolate},
            {MapPatchType.SignalTower, Colors.Azure},
            {MapPatchType.WormHole, Colors.Purple},
            {MapPatchType.Null, Colors.White}
        };

        private void PureDrawMap(int[,] Map)
        {
            lock (drawPicLock)
            {
                for (int i = 0; i < 50; i++)
                {
                    for (int j = 0; j < 50; j++)
                    {
                        switch ((MapPatchType)Map[i, j])
                        {
                            case MapPatchType.RedHome:
                                MapPatchesList[UtilFunctions.getCellIndex(i, j)].PatchColor = Colors.Red; break;  //Red Home
                            case MapPatchType.BlueHome:
                                MapPatchesList[UtilFunctions.getCellIndex(i, j)].PatchColor = Colors.Blue; break; //Blue Home
                            case MapPatchType.Ruin:
                                MapPatchesList[UtilFunctions.getCellIndex(i, j)].PatchColor = Colors.Black; break; // Ruin
                            case MapPatchType.Grass:
                                MapPatchesList[UtilFunctions.getCellIndex(i, j)].PatchColor = Colors.Gray; break; // Grass
                            case MapPatchType.River:
                                MapPatchesList[UtilFunctions.getCellIndex(i, j)].PatchColor = Colors.Brown; break; // River
                            case MapPatchType.Garbage:
                                MapPatchesList[UtilFunctions.getCellIndex(i, j)].PatchColor = Colors.Yellow; break; //Resource
                            case MapPatchType.RecycleBank:
                                MapPatchesList[UtilFunctions.getCellIndex(i, j)].PatchColor = Colors.Orange; break; //RecycleBank
                            case MapPatchType.ChargeStation:
                                MapPatchesList[UtilFunctions.getCellIndex(i, j)].PatchColor = Colors.Chocolate; break; //ChargeStation
                            case MapPatchType.SignalTower:
                                MapPatchesList[UtilFunctions.getCellIndex(i, j)].PatchColor = Colors.Azure; break; //SignalTower
                            default:
                                break;
                        }
                    }
                }
            }
        }

        private void DrawShip()
        {
            for (int i = 0; i < ShipCircList.Count; i++)
            {
                ShipCircList[i].Color = Colors.Transparent;
                ShipCircList[i].Text = "";
            }
            System.Diagnostics.Debug.WriteLine(String.Format("listOfSweeper.Count:{0}", listOfSweeper.Count));
            System.Diagnostics.Debug.WriteLine(String.Format("listOfShip.Count:{0}", ShipCircList.Count));
            for (int i = 0; i < listOfSweeper.Count; i++)
            {
                MessageOfSweeper data = listOfSweeper[i];
                DrawCircLabel shipinfo = ShipCircList[i];
                PointF point = UtilFunctions.getMapCenter(data.X, data.Y);
                shipinfo.X = point.X;
                shipinfo.Y = point.Y;
                System.Diagnostics.Debug.WriteLine(String.Format("shipinfo.X:{0}", data.X));
                System.Diagnostics.Debug.WriteLine(String.Format("shipinfo.Y:{0}", data.Y));
                shipinfo.Text = Convert.ToString(data.Hp);
                long team_id = data.TeamId;
                switch (team_id)
                {
                    case (long)PlayerTeam.Red:
                        System.Diagnostics.Debug.WriteLine("shipinfo.color = red");
                        shipinfo.Color = Colors.DarkRed;
                        break;

                    case (long)PlayerTeam.Blue:
                        System.Diagnostics.Debug.WriteLine("shipinfo.color = blue");

                        shipinfo.Color = Colors.DarkBlue;
                        break;

                    default:
                        System.Diagnostics.Debug.WriteLine("shipinfo.color = black");

                        shipinfo.Color = Colors.DarkGreen;
                        break;
                }
                //shipinfo.Radius = 4.5F;
                //shipinfo.FontSize = 5.5F;
                //shipinfo.TextColor = Colors.White;
                //ShipCircList.Add(shipinfo);
            }
            //shipCircList.Add(
            //    new DrawCircLabel
            //    {
            //        Radius = 4.5F,
            //        Color = Colors.Purple,
            //        Text = "100",
            //        FontSize = 5.5F,
            //        TextColor = Colors.White
            //    }
            //);
        }

        private void DrawBullet()
        {
            for (int i = 0; i < BulletCircList.Count; i++)
            {
                BulletCircList[i].Color = Colors.Transparent;
                BulletCircList[i].Text = "";
            }
            System.Diagnostics.Debug.WriteLine(String.Format("listOfBullet.Count:{0}", listOfBullet.Count));
            System.Diagnostics.Debug.WriteLine(String.Format("BulletCircList.Count:{0}", BulletCircList.Count));
            for (int i = 0; i < listOfBullet.Count; i++)
            {
                MessageOfBullet data = listOfBullet[i];
                DrawCircLabel bulletinfo = BulletCircList[i];
                PointF point = UtilFunctions.getMapCenter(data.X, data.Y);
                bulletinfo.X = point.X;
                bulletinfo.Y = point.Y;
                long team_id = data.TeamId;
                switch (team_id)
                {
                    case (long)PlayerTeam.Red:
                        System.Diagnostics.Debug.WriteLine("bulletinfo.color = red");
                        bulletinfo.Color = Colors.DarkRed;
                        break;

                    case (long)PlayerTeam.Blue:
                        System.Diagnostics.Debug.WriteLine("bulletinfo.color = blue");
                        bulletinfo.Color = Colors.DarkBlue;
                        break;

                    default:
                        System.Diagnostics.Debug.WriteLine("bulletinfo.color = black");
                        bulletinfo.Color = Colors.DarkGreen;
                        break;
                }
                //shipinfo.Radius = 4.5F;
                //shipinfo.FontSize = 5.5F;
                //shipinfo.TextColor = Colors.White;
                //ShipCircList.Add(shipinfo);
            }
        }


        //private void DrawMap()
        //{
        //    //resourceArray = new Label[countMap[(int)MapPatchType.Resource]];
        //    resourcePositionIndex = new (int x, int y)[countMap[(int)MapPatchType.Resource]];
        //    //RecycleBankArray = new Label[countMap[(int)MapPatchType.RecycleBank]];
        //    RecycleBankPositionIndex = new (int x, int y)[countMap[(int)MapPatchType.RecycleBank]];
        //    //ChargeStationArray = new Label[countMap[(int)MapPatchType.ChargeStation]];
        //    ChargeStationPositionIndex = new (int x, int y)[countMap[(int)MapPatchType.ChargeStation]];
        //    //SignalTowerArray = new Label[countMap[(int)MapPatchType.SignalTower]];
        //    SignalTowerPositionIndex = new (int x, int y)[countMap[(int)MapPatchType.SignalTower]];


        //    int counterOfResource = 0;
        //    int counterOfRecycleBank = 0;
        //    int counterOfChargeStation = 0;
        //    int counterOfSignalTower = 0;


        //    int[,] todrawMap;
        //    todrawMap = defaultMap;
        //    for (int i = 0; i < todrawMap.GetLength(0); i++)
        //    {
        //        for (int j = 0; j < todrawMap.GetLength(1); j++)
        //        {
        //            MapPatchType mapPatchType = (MapPatchType)todrawMap[i, j];
        //            switch (mapPatchType)
        //            {
        //                case MapPatchType.RedHome:
        //                    mapPatches[i, j].PatchColor = Colors.Red; break;  //Red Home
        //                case MapPatchType.BlueHome:
        //                    mapPatches[i, j].PatchColor = Colors.Blue; break; //Blue Home
        //                case MapPatchType.Ruin:
        //                    mapPatches[i, j].PatchColor = Colors.Black; break; // Ruin
        //                case MapPatchType.Grass:
        //                    mapPatches[i, j].PatchColor = Colors.Gray; break; // Grass
        //                case MapPatchType.River:
        //                    mapPatches[i, j].PatchColor = Colors.Brown; break; // River
        //                case MapPatchType.Resource:
        //                    mapPatches[i, j].PatchColor = Colors.Yellow; //Resource
        //                    resourcePositionIndex[counterOfResource] = (i, j);
        //                    mapPatches[i, j].Text = "R";
        //                    //resourceArray[counterOfResource] = new Label()
        //                    //{
        //                    //    FontSize = unitFontSize,
        //                    //    WidthRequest = unitWidth,
        //                    //    HeightRequest = unitHeight,
        //                    //    Text = Convert.ToString(-1),
        //                    //    HorizontalOptions = LayoutOptions.Start,
        //                    //    VerticalOptions = LayoutOptions.Start,
        //                    //    HorizontalTextAlignment = TextAlignment.Center,
        //                    //    VerticalTextAlignment = TextAlignment.Center,
        //                    //    BackgroundColor = Colors.Transparent
        //                    //};
        //                    counterOfResource++;
        //                    break;

        //                case MapPatchType.RecycleBank:
        //                    mapPatches[i, j].PatchColor = Colors.Orange; //RecycleBank
        //                    RecycleBankPositionIndex[counterOfRecycleBank] = (i, j);
        //                    mapPatches[i, j].Text = "F";
        //                    //RecycleBankArray[counterOfRecycleBank] = new Label()
        //                    //{
        //                    //    FontSize = unitFontSize,
        //                    //    WidthRequest = unitWidth,
        //                    //    HeightRequest = unitHeight,
        //                    //    Text = Convert.ToString(100),
        //                    //    HorizontalOptions = LayoutOptions.Start,
        //                    //    VerticalOptions = LayoutOptions.Start,
        //                    //    HorizontalTextAlignment = TextAlignment.Center,
        //                    //    VerticalTextAlignment = TextAlignment.Center,
        //                    //    BackgroundColor = Colors.Transparent
        //                    //};
        //                    counterOfRecycleBank++;
        //                    break;

        //                case MapPatchType.ChargeStation:
        //                    mapPatches[i, j].PatchColor = Colors.Chocolate; //ChargeStation
        //                    ChargeStationPositionIndex[counterOfChargeStation] = (i, j);
        //                    mapPatches[i, j].Text = "C";
        //                    //RecycleBankArray[counterOfChargeStation] = new Label()
        //                    //{
        //                    //    FontSize = unitFontSize,
        //                    //    WidthRequest = unitWidth,
        //                    //    HeightRequest = unitHeight,
        //                    //    Text = Convert.ToString(100),
        //                    //    HorizontalOptions = LayoutOptions.Start,
        //                    //    VerticalOptions = LayoutOptions.Start,
        //                    //    HorizontalTextAlignment = TextAlignment.Center,
        //                    //    VerticalTextAlignment = TextAlignment.Center,
        //                    //    BackgroundColor = Colors.Transparent
        //                    //};
        //                    counterOfChargeStation++;
        //                    break;

        //                case MapPatchType.SignalTower:
        //                    mapPatches[i, j].PatchColor = Colors.Azure; //SignalTower
        //                    SignalTowerPositionIndex[counterOfSignalTower] = (i, j);
        //                    mapPatches[i, j].Text = "Fo";
        //                    //SignalTowerArray[counterOfSignalTower] = new Label()
        //                    //{
        //                    //    FontSize = unitFontSize,
        //                    //    WidthRequest = unitWidth,
        //                    //    HeightRequest = unitHeight,
        //                    //    Text = Convert.ToString(-1),
        //                    //    HorizontalOptions = LayoutOptions.Start,
        //                    //    VerticalOptions = LayoutOptions.Start,
        //                    //    HorizontalTextAlignment = TextAlignment.Center,
        //                    //    VerticalTextAlignment = TextAlignment.Center,
        //                    //    BackgroundColor = Colors.Transparent
        //                    //};
        //                    counterOfSignalTower++;
        //                    break;

        //                default:
        //                    break;
        //            }
        //            //MapGrid.Children.Add(mapPatches[i, j]);
        //        }
        //    }
        //}



        //private int FindIndexOfResource(MessageOfResource obj)
        //{
        //    for (int i = 0; i < listOfResource.Count; i++)
        //    {
        //        if (resourcePositionIndex[i].x == obj.X && resourcePositionIndex[i].y == obj.Y)
        //        {
        //            return i;
        //        }
        //    }
        //    return -1;
        //}

        //private int FindIndexOfRecycleBank(MessageOfRecycleBank obj)
        //{
        //    for (int i = 0; i < listOfRecycleBank.Count; i++)
        //    {
        //        if (RecycleBankPositionIndex[i].x == obj.X && RecycleBankPositionIndex[i].y == obj.Y)
        //        {
        //            return i;
        //        }
        //    }
        //    return -1;
        //}

        //private int FindIndexOfChargeStation(MessageOfChargeStation obj)
        //{
        //    for (int i = 0; i < listOfChargeStation.Count; i++)
        //    {
        //        if (ChargeStationPositionIndex[i].x == obj.X && ChargeStationPositionIndex[i].y == obj.Y)
        //        {
        //            return i;
        //        }
        //    }
        //    return -1;
        //}

        //private int FindIndexOfSignalTower(MessgaeOfSignalTower obj)
        //{
        //    for (int i = 0; i < listOfSignalTower.Count; i++)
        //    {
        //        if (SignalTowerPositionIndex[i].x == obj.X && SignalTowerPositionIndex[i].y == obj.Y)
        //        {
        //            return i;
        //        }
        //    }
        //    return -1;
        //}



        private void DrawHome(MessageOfHome data)
        {
            int x = data.X / 1000;
            int y = data.Y / 1000;
            int hp = data.Hp;
            long team_id = data.TeamId;
            int index = UtilFunctions.getCellIndex(x, y);
            System.Diagnostics.Debug.WriteLine(String.Format("Draw Home index: {0}", index));

            MapPatchesList[index].Text = Convert.ToString(hp);
            switch (team_id)
            {
                case (long)PlayerTeam.Red:
                    MapPatchesList[index].PatchColor = PatchColorDict[MapPatchType.RedHome];
                    MapPatchesList[index].TextColor = Colors.White;
                    break;

                case (long)PlayerTeam.Blue:
                    MapPatchesList[index].PatchColor = PatchColorDict[MapPatchType.BlueHome];
                    MapPatchesList[index].TextColor = Colors.White;
                    break;

                default:
                    MapPatchesList[index].PatchColor = Colors.Black;
                    MapPatchesList[index].TextColor = Colors.White;
                    break;
            }
        }

        private void DrawRecycleBank(MessageOfRecycleBank data)
        {
            int x = data.X;
            int y = data.Y;
            int hp = data.Hp;
            long team_id = data.TeamId;
            int index = UtilFunctions.getGridIndex(x, y);
            MapPatchesList[index].Text = Convert.ToString(hp);
            switch (team_id)
            {
                case (long)PlayerTeam.Red:
                    MapPatchesList[index].PatchColor = PatchColorDict[MapPatchType.RecycleBank];
                    MapPatchesList[index].TextColor = Colors.Red;
                    break;

                case (long)PlayerTeam.Blue:
                    MapPatchesList[index].PatchColor = PatchColorDict[MapPatchType.RecycleBank];
                    MapPatchesList[index].TextColor = Colors.Blue;
                    break;

                default:
                    MapPatchesList[index].PatchColor = Colors.Black;
                    MapPatchesList[index].TextColor = Colors.White;
                    break;
            }
        }

        private void DrawChargeStation(MessageOfChargeStation data)
        {
            int x = data.X;
            int y = data.Y;
            int hp = data.Hp;
            long team_id = data.TeamId;
            int index = UtilFunctions.getGridIndex(x, y);
            MapPatchesList[index].Text = Convert.ToString(hp);
            switch (team_id)
            {
                case (long)PlayerTeam.Red:
                    MapPatchesList[index].PatchColor = PatchColorDict[MapPatchType.ChargeStation];
                    MapPatchesList[index].TextColor = Colors.Red;
                    break;

                case (long)PlayerTeam.Blue:
                    MapPatchesList[index].PatchColor = PatchColorDict[MapPatchType.ChargeStation];
                    MapPatchesList[index].TextColor = Colors.Blue;
                    break;

                default:
                    MapPatchesList[index].PatchColor = Colors.Black;
                    MapPatchesList[index].TextColor = Colors.White;
                    break;
            }
        }

        private void DrawSignalTower(MessageOfSignalTower data)
        {
            int x = data.X;
            int y = data.Y;
            int hp = data.Hp;
            long team_id = data.TeamId;
            int index = UtilFunctions.getGridIndex(x, y);
            MapPatchesList[index].Text = Convert.ToString(hp);
            switch (team_id)
            {
                case (long)PlayerTeam.Red:
                    MapPatchesList[index].PatchColor = PatchColorDict[MapPatchType.SignalTower];
                    MapPatchesList[index].TextColor = Colors.Red;
                    break;

                case (long)PlayerTeam.Blue:
                    MapPatchesList[index].PatchColor = PatchColorDict[MapPatchType.SignalTower];
                    MapPatchesList[index].TextColor = Colors.Blue;
                    break;

                default:
                    MapPatchesList[index].PatchColor = Colors.Black;
                    MapPatchesList[index].TextColor = Colors.White;
                    break;
            }
        }

        private void DrawWormHole(MessageOfBridge data)
        {
            int x = data.X;
            int y = data.Y;
            int hp = data.Hp;
            int index = UtilFunctions.getGridIndex(x, y);
            MapPatchesList[index].Text = Convert.ToString(hp);
            MapPatchesList[index].PatchColor = PatchColorDict[MapPatchType.WormHole];
            MapPatchesList[index].TextColor = Colors.White;
        }

        private void DrawResource(MessageOfGarbage data)
        {
            int x = data.X;
            int y = data.Y;
            int hp = data.Progress;
            int index = UtilFunctions.getGridIndex(x, y);
            MapPatchesList[index].Text = Convert.ToString(hp);
            MapPatchesList[index].PatchColor = PatchColorDict[MapPatchType.Garbage];
            MapPatchesList[index].TextColor = Colors.White;
        }

        
        private void DrawSweeper(MessageOfSweeper data, ICanvas canvas)
        {
            PointF point = UtilFunctions.getMapCenter(data.X, data.Y);
            float x = point.X;
            float y = point.Y;
            int hp = data.Hp;
            long team_id = data.TeamId;
            switch (team_id)
            {
                case (long)PlayerTeam.Red:
                    canvas.FillColor = Colors.Red;
                    break;

                case (long)PlayerTeam.Blue:
                    canvas.FillColor = Colors.Blue;
                    break;

                default:
                    canvas.FillColor = Colors.Black;
                    break;
            }
            canvas.FillCircle(x, y, (float)4.5);
            canvas.FontSize = 5.5F;
            canvas.FontColor = Colors.White;
            canvas.DrawString(Convert.ToString(hp), x - 5, y - 5, 10, 10, HorizontalAlignment.Left, VerticalAlignment.Top);
        }

        private bool isClientStocked = false;
        private bool hasDrawn = false;
        private bool getMapFlag = false;
        public readonly object drawPicLock = new();
        //private bool isPlaybackMode;
        //private double unit;
        //private double unitFontSize = 10;
        //private double unitHeight = 10.6;
        //private double unitWidth = 10.6;



        public ObservableCollection<MapPatch> mapPatchesList;
        public ObservableCollection<MapPatch> MapPatchesList
        {
            get
            {
                return mapPatchesList ??= new ObservableCollection<MapPatch>();
            }
            set
            {
                mapPatchesList = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<DrawCircLabel> shipCircList;
        public ObservableCollection<DrawCircLabel> ShipCircList
        {
            get
            {
                return shipCircList ??= new ObservableCollection<DrawCircLabel>();
            }
            set
            {
                shipCircList = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<DrawCircLabel> bulletCircList;
        public ObservableCollection<DrawCircLabel> BulletCircList
        {
            get
            {
                return bulletCircList ??= new ObservableCollection<DrawCircLabel>();
            }
            set
            {
                bulletCircList = value;
                OnPropertyChanged();
            }
        }





        //private MapPatch[] mapPatches_ = new MapPatch[2500];
        //public MapPatch[] MapPatches_
        //{
        //    get
        //    {
        //        return mapPatches_ ??= new MapPatch[2500];
        //    }
        //    set
        //    {
        //        mapPatches_ = value;
        //        OnPropertyChanged();
        //    }
        //}

        private MapPatch[,] mapPatches_ = new MapPatch[50, 50];
        public MapPatch[,] MapPatches_
        {
            get
            {
                return mapPatches_;
            }
            set
            {
                mapPatches_ = value;
                OnPropertyChanged();
            }
        }

        //private readonly BoxView[,] mapPatches = new BoxView[50, 50];
        private readonly double characterRadiusTimes = 400;
        private readonly double bulletRadiusTimes = 200;

        private int mapHeight = 500;
        public int MapHeight
        {
            get
            {
                return mapHeight;
            }
            set
            {
                mapHeight = value;
                OnPropertyChanged();
            }
        }

        private int mapWidth = 500;
        public int MapWidth
        {
            get
            {
                return mapWidth;
            }
            set
            {
                mapWidth = value;
                OnPropertyChanged();
            }
        }

        public Command MoveUpCommand { get; }
        public Command MoveDownCommand { get; }
    }
}
