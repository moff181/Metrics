   M S B A M L   `   `   `  ����H  DMoffs Metrics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:Moff_s_MetricsMoff_s_Metrics  5   	       Moff_s_Metrics.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:Moff_s_Metrics     5   	     � Title$  Moff's Metrics��5   	   $	��450��6    $	��800��6-   .��5	      �� .+�5
   
   �  � ColumnDefinitions 5      �� 5      $��* �6#   �� 5      $��* �6#   .+�5      �� -   5         ��Name$ ProjectSelectButton��5   >   $��Select Project��6   $��0��6.   6[   �� 5      -   5        $ RefreshButton��5   7   $��Refresh��6   $��1��6'   6N   5      �� 5   
   -   5         ��Name$ SelectedProject��5      $��2,0,0,0q�6-   �� 6?   .�6D   No project selected.6Z   6a   � 5   
    5      �� 5      $	��130 �6#   �� 5      .+�5      �� -   5        $��ExcludedDirectoriesLabel��5   $   $��0��6   $��-3,0,0,0q�6D   .��5      �� .�6   Excluded Directories:6.   5      �� 5      -   5        $ ��ExcludedDirectoriesTextBox��5   7   $��1��6   $��0,5,0,0q�6&   $	��Top=�6Y   5       �� 5"   
   -   5         ��Name$ LanguageSelectionPanel��5"       �� Orientation$ 
Horizontal=�65   .+�5#      �� -   5         ��Name$ LanguageSelectionLabel��5#      $��-2, 0, 0, 0q�64   $
��Bold"�6I   .��6[   Language Selection:6p   �� 5%      -   5         ��Name$ LanguageSelectionCombo��5%      .@�5&      ��  �� 
IsSelected . $	��C++��61   �� 5'      $��C#��6   5(      5)      �� 5+   
   -	   5        $ 	CodeLines��5+      $��2,0,0,0q�6'   �� 69   .�6>   Code Lines:6K    N/A6V   �� 5,   
   -
   5        $ HeaderCodeLines��5,      $��2,0,0,0q�6-   �� 6?   .�6D   Header Code Lines:6X    N/A6c   �� 5-   
   -   5        $ CPPCodeLines��5-      $��2,0,0,0q�6*   �� 6<   .�6A   CPP Code Lines:6R    N/A6]   �� 5.   
   -   5        $ Comments��5.      $��2,0,0,0q�6&   �� 68   .�6=   	Comments:6H    N/A6S   �� 5/   
   -   5        $ 
EmptyLines��5/      $��2,0,0,0q�6(   �� 6:   .�6?   Empty Lines:6M    N/A6X   50      51      