   M S B A M L   `   `   `  ����N  JPOEDuplicateScanner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:POEDuplicateScannerPOEDuplicateScanner  5   	   &     POEDuplicateScanner.QuickyWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:POEDuplicateScanner     5   	   -   5          � AllowsTransparency  .  � Title$ QuickyWindow��5   	   $��137.033��6   $	��384��6/    � 
ResizeMode$ CanResizeWithGrip=�6;    � WindowStyle$
 None=�6Z    � Topmost . ��5	      �� 5
   
    �� Opacity$	 0.5Q�6    �� Color$ White��6(   .��5      �  � RowDefinitions 5   
   �� 5      $
��33.6 �6   �� 5      $
��31.2 �6   �� 5      .+�5   
   �� -   5         ��Name$ 
btnRefresh��5      $��F��6%   $
��Left=�61   $��	3,5,0,3.6q�6L   $��30��6_   6j   �� 5   
   -   5        $ btnChaos��5      $��C��6#   $
��Left=�6/   $��
38,5,0,3.6q�6J   $��30��6^   6i   �� 5   
   -   5        $ btnRegal��5      $��R��6#   $
��Left=�6/   $��
73,5,0,3.6q�6J   $��30��6^   6i   �� 5   
   -   5        	 ��Name$	 ddlTabs��5      $��206,5,0,3.6q�6$   $
��Left=�69   $	��120��6T   �� 5   
   -   5        $ btnMinimize��5      $��<��6&   $��Right=�65   $��	0,5,4,3.6q�6Q   $��30��6d   6o   �� 5   
   -   5        $	��lbl��5   C   $��Label��6   $��3,1.4,11,3.8q�6    $��1��66   6C   �� 5   
   -   5        
 ��Name$
 lblLogs��5      $��Label��6!   $��3,1.2,4,4.6q�61   $��2��6F   5      5      