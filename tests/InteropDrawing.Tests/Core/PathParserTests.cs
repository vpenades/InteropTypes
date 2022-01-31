﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using InteropDrawing.Backends;

using InteropTypes.Graphics.Drawing;

using NUnit.Framework;

namespace InteropDrawing
{
    public class PathParserTests
    {
        // https://github.com/wwwMADwww/ManuPathLib/blob/master/ManuPathTest/svg/polygon%202.svg
        // [TestCase("M 9.63491,6.7967016 32.405716,11.47376 39.601628,38.63927 4.4367316,37.033764")]
        // [TestCase("M 60.135988,6.7967016 82.906794,11.47376 90.102706,38.63927 54.937809,37.033764")]
        // [TestCase("M 9.7556821,57.177007 32.526488,61.854066 39.7224,89.019576 4.5575037,87.41407 Z")]
        // [TestCase("m 60.256759,57.177007 22.770806,4.677059 7.195912,27.16551 -35.164896,-1.605506 z")]
        // [TestCase("m 117.17262,88.357143 c -0.25199,-1.007938 -0.3701,-2.059162 -0.75595,-3.023809 -0.13235,-0.330872 -0.55828,-0.459444 -0.75596,-0.755955 -0.31255,-0.46882 -0.46605,-1.028747 -0.75595,-1.511903 -1.79903,-2.998388 -1.89819,-2.284473 -3.77976,-6.047621 -0.60686,-1.213715 -0.86211,-2.588477 -1.51191,-3.779761 -2.4073,-4.41339 -3.31264,-4.824544 -6.80357,-8.315475 -1.25992,-1.259922 -2.44803,-2.596002 -3.77976,-3.779764 -0.835761,-0.742897 -3.077015,-2.537576 -4.535713,-3.023809 -0.478107,-0.159369 -1.007938,0 -1.511906,0 -0.503967,0 -1.06114,-0.22538 -1.511903,0 -0.637476,0.318739 -1.007938,1.007938 -1.511906,1.511906 -1.76389,1.763887 -3.634401,3.427243 -5.291666,5.291667 -1.220936,1.373552 -4.053094,4.63178 -4.535715,6.80357 -0.273315,1.229918 0,2.519841 0,3.779763 0,1.638395 -0.164831,3.001073 0.755954,4.535715 0.366689,0.61115 1.007935,1.007935 1.511903,1.511903 0.251984,0.251984 0.437216,0.596585 0.755952,0.755952 0.225382,0.112691 0.530571,-0.112689 0.755954,0 0.318736,0.159369 0.41788,0.643263 0.755952,0.755954 2.682367,0.894122 0.382484,-1.225042 3.023809,0.755952 1.140351,0.855263 1.88346,2.168546 3.023809,3.023809 0.450762,0.338071 1.061143,0.41788 1.511906,0.755951 1.140349,0.855263 1.88346,2.168549 3.023809,3.023812 0.736589,0.552442 1.805421,0.587034 2.267857,1.511903 0.112692,0.225383 -0.178181,0.577774 0,0.755952 0.377976,0.377978 1.889882,-0.377976 2.267858,0 0.178178,0.178181 0,0.50397 0,0.755954 0.251986,0 0.530566,-0.112691 0.755956,0 0.31873,0.159369 0.50396,0.503968 0.75595,0.755952 0.25198,0.251984 0.59658,0.437213 0.75595,0.755952 0.11269,0.225382 -0.0797,0.5169 0,0.755954 1.09402,3.282071 0.75595,-0.519338 0.75595,3.77976 0,1.491723 0.36527,4.646103 -0.75595,6.047623 -0.56756,0.70945 -1.5119,1.00793 -2.267856,1.5119 -0.755954,0.50397 -1.511906,1.00794 -2.267858,1.5119 -0.755951,0.50398 -1.417164,1.1929 -2.267857,1.51191 -2.674782,1.00304 -4.789874,0.75595 -7.559524,0.75595 -1.007936,0 -2.015874,0 -3.023809,0 -1.511906,0 -3.035486,0.18753 -4.535715,0 -0.559102,-0.0699 -1.007936,-0.50397 -1.511904,-0.75595 -1.007938,-0.50397 -1.994029,-1.05422 -3.023809,-1.51191 -7.073437,-3.14374 -1.007372,0.42045 -6.047621,-3.77976 -2.854634,-2.37886 -2.362771,-0.55198 -4.535712,-4.53571 -0.918943,-1.68473 -1.409631,-3.575212 -2.267857,-5.291667 -0.657096,-1.314189 -1.489126,-2.533791 -2.267858,-3.779761 -0.481526,-0.770443 -1.2246,-1.40594 -1.511906,-2.267858 -0.170444,-0.511336 0,-3.620494 0,-4.535715 0,-0.251984 0.178181,-0.577773 0,-0.755951 -0.178178,-0.178181 -0.554365,-0.151191 -0.755951,0 -1.858542,1.393907 -3.589256,2.954903 -5.291667,4.535715 -1.827964,1.697394 -3.527777,3.527777 -5.291667,5.291666 -2.267857,2.267858 -4.834519,4.271934 -6.803572,6.803571 -1.592768,2.04784 -2.675102,4.45617 -3.779761,6.80357 -1.323049,2.81148 -3.361743,8.9601 -3.779761,12.09524 -0.233124,1.74841 0,3.52778 0,5.29166 0,1.76389 -0.427807,3.58045 0,5.29167 1.478545,5.91418 1.524352,2.52814 4.535713,4.53572 0.209664,0.13977 -0.178179,0.57777 0,0.75595 0.822552,0.82255 0.987557,-1.34144 1.511906,0.75595 0.629959,2.51984 -0.419973,1.67989 0,3.77976 0.633367,3.16685 0.781288,1.56258 2.267857,4.53572 0.521364,1.04273 -0.9155,0.67617 0.755952,1.5119 0.548367,0.27419 1.375309,-0.20957 1.511906,-0.75595 0.183343,-0.73338 0,-1.5119 0,-2.26786 0,-0.50397 -0.159369,-1.0338 0,-1.5119 0.112688,-0.33808 0.558278,-0.45944 0.755951,-0.75596 0.312547,-0.46882 0.251984,-1.25991 0.755952,-1.5119 0.676145,-0.33807 1.511906,0 2.267857,0 1.837331,0 3.248226,-0.41933 4.535715,1.5119 0.576311,0.86447 0.19816,2.14729 0.755952,3.02382 1.247259,1.95997 3.213338,3.38156 4.535715,5.29166 2.71862,3.9269 2.267857,3.74595 2.267857,7.55952 0,1.91752 0.318736,3.57217 -0.755954,5.29167 -3.620939,5.79351 -1.495054,2.50861 -4.535712,4.53572 -0.296511,0.19767 -1.07469,0.91532 -0.755954,0.75595 3.455932,-1.72797 4.737782,-4.5718 7.559524,-7.55952 2.446848,-2.59079 4.97592,-5.1051 7.559524,-7.55953 4.915641,-4.66986 10.079365,-9.07143 15.119048,-13.60714 2.519841,-2.26786 5.162359,-4.40641 7.559524,-6.80357 1.391626,-1.39163 2.388132,-3.14409 3.779761,-4.53572 0.398423,-0.39842 1.173832,-1.20671 1.511906,-0.75595 0.770921,1.0279 0.294708,2.58053 0.755951,3.77976 0.809072,2.10359 2.064142,4.00833 3.023814,6.04762 1.05672,2.24554 1.7655,4.66446 3.0238,6.80357 1.27761,2.17194 3.1014,3.97583 4.53572,6.04762 0.83634,1.20805 1.32723,2.65101 2.26786,3.77976 0.36071,0.43286 1.11348,0.35753 1.5119,0.75596 0.10661,0.1066 0.0775,2.11279 0,2.26785 -0.40631,0.81263 -0.93027,1.5699 -1.5119,2.26786 -2.61808,3.14169 -2.64779,2.29798 -6.80358,4.53572 -1.78873,0.96316 -3.54961,1.97857 -5.291662,3.0238 -2.038443,1.22307 -3.921374,2.71664 -6.047618,3.77976 -3.006614,1.50331 -2.775276,-0.3728 -4.535715,2.26786 -0.139774,0.20967 0,0.50397 0,0.75595 0,3.3193 -0.38716,1.8167 4.535715,5.29167 2.684123,1.89468 5.40055,3.77591 8.31548,5.29167 1.6684,0.86757 20.44419,8.70182 21.92261,9.07143 13.756,3.439 0.88636,-3.13384 11.33929,1.5119 0.83024,0.369 1.45523,1.10559 2.26786,1.51191 0.22538,0.11269 0.75595,-0.25199 0.75595,0 0,0.35636 -0.50397,0.50396 -0.75595,0.75595 -0.50397,0.50397 -0.97077,1.04807 -1.51191,1.5119 -1.22505,1.05005 -2.36329,2.25119 -3.77976,3.02381 -1.39909,0.76314 -3.03487,0.97589 -4.53571,1.51191 -6.65904,2.37822 -6.05406,2.27072 -12.85119,5.29167 -2.26786,1.00793 -4.53572,2.01587 -6.80358,3.0238 -1.5119,0.50397 -3.06461,0.89895 -4.53571,1.51191 -1.56033,0.65014 -2.975381,1.61772 -4.535714,2.26786 -1.34426,0.5601 -5.712743,1.96005 -7.559524,2.26785 -0.497112,0.0829 -1.007935,0 -1.511906,0 -1.763887,0 -3.527777,0 -5.291666,0 -0.755952,0 -1.511904,0 -2.267855,0 -0.755954,0 -1.573027,0.29779 -2.267858,0 -1.158049,-0.49631 -1.975493,-1.56898 -3.023809,-2.26785 -1.222539,-0.81503 -2.504045,-1.53888 -3.779763,-2.26786 -0.489215,-0.27955 -1.159918,-0.31597 -1.511903,-0.75595 -0.703975,-0.87997 -1.007938,-2.01588 -1.511906,-3.02381 -0.251984,-0.50397 -0.357532,-1.11349 -0.755952,-1.51191 -0.642437,-0.64243 -1.700297,-0.80245 -2.267857,-1.5119 -1.180873,-1.47609 -0.225124,-2.49298 -1.511906,-3.77977 -0.178179,-0.17817 -0.577771,-0.17817 -0.755952,0 -0.178181,0.17819 0.09359,0.522 0,0.75596 -0.418523,1.0463 -0.977638,2.0316 -1.511906,3.02381 -1.229973,2.28423 -2.619539,4.48312 -3.77976,6.80357 -0.778312,1.55662 -5.173581,12.3198 -5.291667,12.85119 -0.437303,1.96787 -0.105952,4.03453 0,6.04762 0.146285,2.77941 -0.08794,5.66324 0.755952,8.31547 1.471303,4.6241 7.786661,13.03523 10.583333,16.63096 1.388181,1.7848 6.196383,7.37615 8.315476,9.07142 1.147336,0.91789 2.533792,1.48912 3.779763,2.26787 0.541959,0.33872 2.816339,2.027 3.779761,2.26785 0.733382,0.18335 1.511905,0 2.267857,0 1.007938,0 2.148676,0.50009 3.023809,0 2.01721,-1.15269 4.564161,-5.32011 6.047621,-6.80357 1.391626,-1.39163 3.144086,-2.38814 4.535712,-3.77976 4.62094,-4.62094 2.65929,-4.3062 7.55952,-8.31548 1.40635,-1.15065 3.19119,-1.80151 4.53572,-3.02381 2.32654,-2.11503 5.76488,-7.43453 7.55952,-9.82738 0.9681,-1.29079 2.19368,-2.39621 3.02381,-3.77976 0.69816,-1.1636 0.96079,-2.53974 1.51191,-3.77976 0.45768,-1.02978 0.96463,-2.03872 1.5119,-3.02381 0.71356,-1.28441 1.75195,-2.40401 2.26786,-3.77976 0.26543,-0.70783 -0.45357,-1.6631 0,-2.26786 0.4781,-0.63748 1.60484,-0.31395 2.26786,-0.75596 0.20966,-0.13977 -0.17818,-0.57777 0,-0.75595 0.17818,-0.17818 0.50396,0 0.75595,0 0.25198,0 0.53057,-0.11269 0.75595,0 0.31874,0.15937 0.47768,0.53334 0.75595,0.75595 3.35979,2.68783 2.93982,1.5959 5.29167,5.29167 1.0907,1.71395 1.95907,3.56147 3.02381,5.29167 3.08953,5.02048 2.37257,3.47279 5.29167,7.55952 0.52808,0.73931 1.10559,1.45523 1.5119,2.26786 0.22538,0.45076 -0.35636,1.15554 0,1.5119 0.17818,0.17818 0.50397,0 0.75595,0 0.25199,0 0.53057,0.11269 0.75596,0 2.81532,-1.40766 2.02791,-1.68993 4.53571,-3.77976 0.69796,-0.58163 1.62542,-0.86947 2.26786,-1.5119 0.8909,-0.8909 1.48079,-2.03998 2.26785,-3.02381 2.12369,-2.65462 4.70584,-5.09521 6.04762,-8.31548 2.06571,-4.9577 1.51191,-5.95416 1.51191,-11.33928 0,-8.71889 0.66301,-7.08239 -2.26786,-15.875 -0.85823,-2.57469 -2.20655,-4.97155 -3.02381,-7.55953 -0.69958,-2.21534 -0.85638,-4.57479 -1.5119,-6.80357 1.54424,-4.12405 -2.81694,-5.42699 -3.77977,-8.31547 0,-5.36177 0.53454,-0.44283 -0.75595,-3.02382 -0.16143,-0.32285 0,-3.48092 0,-3.77976 0,-2.77426 -0.33807,-2.5167 1.51191,-5.29166 0.39534,-0.59302 1.06667,-0.95537 1.5119,-1.51191 0.56756,-0.70945 0.94434,-1.5584 1.51191,-2.26785 1.09557,-1.36947 2.26856,-2.11709 3.77976,-3.02382 3.30265,-1.98159 -0.5472,0.65158 3.77976,-1.5119 0.31874,-0.15937 0.41788,-0.64326 0.75595,-0.75595 0.43004,-0.14335 3.06778,0 3.77976,0 0.25199,0 0.57778,0.17818 0.75596,0 0.0742,-0.0742 0.10339,-1.8543 0,-2.26786 -0.3208,-1.2832 -1.51896,-4.45956 -2.26786,-5.29166 -1.55412,-1.72681 -3.64893,-2.89299 -5.29167,-4.53572 -1.89407,-1.89408 -3.16852,-4.41443 -5.29166,-6.04762 -1.2632,-0.97169 -3.0435,-0.95232 -4.53572,-1.5119 -0.52758,-0.19785 -0.98875,-0.54669 -1.5119,-0.75596 -1.72375,-0.68949 -2.21526,-0.75595 -3.77976,-0.75595 -0.50397,0 -1.0338,-0.15937 -1.51191,0 -10.95136,3.65045 2.38368,-0.43588 -4.53571,3.02381 -0.92927,0.46464 -2.00503,0.5522 -3.02381,0.75595 -0.94244,0.18849 -1.37351,-0.30878 -2.26786,-0.75595 -2.8608,-1.4304 -0.67746,-0.057 -3.77976,-3.77976 -2.18707,-2.62448 -5.49558,-4.05164 -7.55953,-6.80357 -5.60055,-7.4674 -0.7533,-0.74669 -2.26785,-6.04762 -0.30959,-1.08355 -1.26745,-1.92374 -1.51191,-3.02381 -0.27331,-1.22992 0.15628,-2.52957 0,-3.77976 -0.0988,-0.79069 -0.56269,-1.49481 -0.75595,-2.26786 -0.20767,-0.83068 0.0457,-2.55033 0.75595,-3.02381 0.41933,-0.27955 1.00794,0 1.51191,0 0.25198,0 0.50397,0 0.75595,0 1.00794,0 2.01587,0 3.02381,0 0.75595,0 1.5241,-0.13523 2.26786,0 2.04439,0.37171 4.01582,1.07652 6.04762,1.5119 1.49873,0.32116 3.00684,0.64675 4.53571,0.75596 2.01075,0.14362 4.03174,0 6.04762,0 2.26786,0 4.53571,0 6.80357,0 1.76389,0 3.52778,0 5.29167,0 0.50397,0 1.0338,0.15937 1.5119,0 2.21047,-0.73683 1.38327,-0.97314 3.02381,-3.02381 0.44523,-0.55654 1.00794,-1.00794 1.51191,-1.51191 1.65543,-1.65543 3.46512,-3.15048 4.53571,-5.291663 0.35636,-0.712718 0.62495,-1.481854 0.75595,-2.267857 0.12428,-0.745667 0,-1.511903 0,-2.267857 0,-2.267858 0,-4.535713 0,-6.80357 0,-1.490877 0.42932,-4.106397 -0.75595,-5.291667 -0.39842,-0.398423 -1.06114,-0.41788 -1.51191,-0.755954 -0.85526,-0.641445 -1.37833,-1.674839 -2.26785,-2.267855 -0.14081,-0.09388 -2.97537,-1.463466 -3.02381,-1.511906 -0.17818,-0.178181 0.17818,-0.577773 0,-0.755952 -0.17818,-0.178181 -0.50397,0 -0.75595,0 -0.50397,0 -1.00794,0 -1.51191,0 -2.01587,0 -4.03174,0 -6.04762,0 -0.75595,0 -1.5507,-0.239053 -2.26786,0 -0.33807,0.112689 -0.41788,0.643261 -0.75595,0.755952 -3.44131,0 -0.0432,-0.356359 -2.26785,0.755952 -0.4101,0.205046 -2.81697,-0.206846 -3.02381,0 -0.50297,0.50297 0.50297,1.008935 0,1.511906 -0.37798,0.377975 -1.88989,-0.377976 -2.26786,0 -0.17818,0.178178 0.17818,0.577773 0,0.755951 -0.35636,0.35636 -1.06114,-0.22538 -1.51191,0 -0.63747,0.318738 -0.87443,1.193168 -1.5119,1.511906 -0.22538,0.112689 -0.5169,-0.07968 -0.75595,0 -0.53454,0.178178 -0.97737,0.577771 -1.51191,0.755952 -0.4781,0.159369 -1.0338,-0.159369 -1.5119,0 -2.60593,0.868643 -0.55998,0.755951 -2.26786,0.755951 -1.77284,0 0.35636,-0.178178 -1.5119,0.755955 -0.60477,0.302379 -1.6631,-0.302382 -2.26786,0 -0.31874,0.159366 -0.43722,0.596582 -0.75595,0.755951 -0.29973,0.149863 -2.11332,-0.154546 -2.26786,0 -0.17818,0.178179 0.17818,0.577773 0,0.755952 -0.17818,0.178181 -0.50397,0 -0.75595,0 -0.25199,0 -0.75596,0.251984 -0.75596,0 0,-0.251984 1.00794,0 0.75596,0 -0.50397,0 -1.00794,0 -1.51191,0 -0.50397,0 -1.04398,-0.187169 -1.5119,0 -1.04631,0.418523 -3.29713,2.605167 -3.02381,1.511906 0.47732,-1.9093 2.68808,-2.898191 3.77976,-4.535715 0.44201,-0.663014 0.39959,-1.555139 0.75595,-2.267858 0.31874,-0.637474 1.28653,-0.83576 1.51191,-1.511903 0.53659,-1.60977 -0.77352,-2.250289 0.75595,-3.779763 0.17818,-0.178179 0.57777,0.178181 0.75595,0 0.20685,-0.206846 -0.20505,-2.613716 0,-3.02381 0.15937,-0.318738 0.59659,-0.437216 0.75595,-0.755951 0.11269,-0.225383 -0.17818,-0.577773 0,-0.755954 0.17818,-0.178179 0.57778,0.178181 0.75596,0 1.56409,-1.564093 -2.39014,0.06114 0.75595,-1.511904 0.22538,-0.112691 0.57777,0.178179 0.75595,0 0.17818,-0.178181 0,-0.503967 0,-0.755951 0.25198,0 0.57777,0.178178 0.75595,0 0.17818,-0.178181 -0.17818,-0.577773 0,-0.755954 0.17818,-0.178179 0.57777,0.178181 0.75596,0 0.17817,-0.178179 -0.1127,-0.530572 0,-0.755952 0.45357,-0.907143 1.81428,-1.360715 2.26785,-2.267858 0.11269,-0.22538 -0.17818,-0.577773 0,-0.755951 0.17818,-0.178181 0.53057,0.112691 0.75596,0 0.31873,-0.159369 0.43721,-0.596585 0.75595,-0.755952 0.45076,-0.225382 1.15554,0.35636 1.5119,0 0.17818,-0.178181 -0.17818,-0.577773 0,-0.755954 0.17818,-0.178178 0.50397,0 0.75595,0 0.25199,0 0.75596,-0.251984 0.75596,0 0,0.251984 -0.75596,0.251984 -0.75596,0 0,-0.356359 0.59659,-0.437213 0.75596,-0.755952 0.0994,-0.198771 0,-2.628505 0,-3.023809 0,-0.251984 0.17818,-0.577773 0,-0.755951 -0.17818,-0.178181 -0.57777,0.178178 -0.75596,0 -0.17817,-0.178181 0.17819,-0.577774 0,-0.755955 -0.17817,-0.178178 -0.57777,0.178181 -0.75595,0 -0.17818,-0.178178 0.17818,-0.577773 0,-0.755951 -0.17818,-0.178181 -0.53057,0.112691 -0.75595,0 -0.31874,-0.159369 -0.43722,-0.596583 -0.75595,-0.755952 -0.16943,-0.08472 -2.84417,0 -3.02381,0 -1.51191,0 -3.02381,0 -4.53572,0 -0.32858,0 -3.63153,-0.08894 -3.77976,0 -1.08037,0.648224 -1.97549,1.568979 -3.02381,2.267858 -0.93764,0.625094 -2.08616,0.886806 -3.02381,1.511903 -0.29651,0.197673 -0.45944,0.558279 -0.75595,0.755951 -0.46882,0.31255 -1.11348,0.357532 -1.5119,0.755955 -0.17818,0.178178 0.17817,0.57777 0,0.755951 -0.39843,0.398423 -1.11349,0.357532 -1.51191,0.755952 -0.0304,0.03042 0,2.746198 0,3.023809 0,2.015874 0,4.031747 0,6.047621 0,0.251984 0,0.503968 0,0.755951 0,0.251984 -0.17818,0.577774 0,0.755952 0.17818,0.178181 0.57777,-0.178178 0.75595,0 2.95029,2.950282 -0.17498,0.405985 0.75596,2.267858 0.31873,0.637476 1.19316,0.874429 1.5119,1.511905 0.13669,0.273379 0,3.253965 0,3.779761 0,0.251984 -0.17818,0.577773 0,0.755952 0.17818,0.178181 0.57777,-0.178179 0.75595,0 0.17818,0.178181 0.25199,0.755954 0,0.755954 -0.25198,0 0,-1.007938 0,-0.755954 0,0.50397 0,1.007938 0,1.511906 0,2.371018 -0.0918,1.328227 1.51191,4.535715 z")]
        [TestCase("M 50,50 C 50,20 60,20 60,50     60,80  70,80 70,50 z")]
        [TestCase("M 50,50 C 50,20 60,20 60,50     60,80  70,80 70,50 z")]
        [TestCase("M300,200 h-150 a150,150 0 1,0 150,-150 z " +
            "M275,175 v-150 a150,150 0 0,0 -150,150 z M600,350 l 50,-25 " +
            "a25, 25 -30 0, 1 50, -25 l 50, -25 " +
            "a25, 50 -30 0, 1 50, -25 l 50, -25 " +
            "a25, 75 -30 0, 1 50, -25 l 50, -25 " +
            "a25, 100 -30 0, 1 50, -25 l 50, -25")]
        public void ParsePathAndFill(string path)
        {
            using (var svg = SVGSceneDrawing2D.CreateGraphic())
            {
                svg.DrawPath(System.Numerics.Matrix3x2.Identity, path, (System.Drawing.Color.Red, System.Drawing.Color.Blue, 2));

                var document = svg.ToSVGContent();
                var docPath = TestContext.CurrentContext.UseFilePath("document.svg");

                System.IO.File.WriteAllText(docPath, document);
                TestContext.AddTestAttachment(docPath);
            }
        }
    }
}
