# MEMO

## 關於修改Texture
目前寫法有兩種
### 重複呼叫SetPixel
```csharp
//get pixels return an 1d-array and start at left-bottom row by row
//make start point at left-bottom
Vector2 pixelUV;
pixelUV.x = texCoord.x * paintTex.width - splashTex.width * .5f;
pixelUV.y = texCoord.y * paintTex.height + splashTex.height * .5f;
for (int i = 0; i < splashTex.height; i++) {
    for (int j = 0; j < splashTex.width; j++) {
        if (splashColors[i * splashTex.width + j].a == 0)
            continue;
        paintTex.SetPixel ((int) pixelUV.x + j, (int) pixelUV.y - i, color);
    }
}
paintTex.Apply ( );
```
運行時的Profile
![](https://i.imgur.com/MnyC6sa.png)
### 呼叫SetPixels
```csharp
//get pixels return an 1d-array and start at left-bottom row by row
//make start point at left-bottom
Color32[ ] paintTexColors = paintTex.GetPixels32 ( );
Vector2 pixelUV;
pixelUV.x = texCoord.x * paintTex.width - splashTex.width * .5f;
pixelUV.y = texCoord.y * paintTex.height + splashTex.height * .5f;
for (int i = 0; i < splashTex.height; i++) {
    for (int j = 0; j < splashTex.width; j++) {
        if (splashColors[i * splashTex.width + j].a == 0)
            continue;
        paintTexColors[((int) pixelUV.y - i) * paintTex.width + (int) pixelUV.x + j] = color;
        //paintTex.SetPixel ((int) pixelUV.x + j, (int) pixelUV.y - i, color);
    }
}
paintTex.SetPixels32 (paintTexColors);
paintTex.Apply ( );
```
運行的Profile
![](https://i.imgur.com/x80Ry1R.png)

### 比較
目前兩種作法由重複呼叫`SetPixel`在執行效率上比較快  
雖然官方文件表示比起重複呼叫`SetPixel`呼叫一次`SetPixels`比較快  
但測試出來的結果卻相反  
推測在於兩者差了一個`GetPixles`的動作  

由於`GetPixels`的動作會造成更多的效能耗損  
所以透過`SetPixels`節省的效能就被抵消了  

另外考慮使用`Color32`而非`Color`  
比起浮點數的運算 整數的速度會比較快

而最後所呼叫的`Apply`Method才是造成運行速度無法提昇的原因  
必須考慮其他的解決辦法

---

## New Input System how to get mouse position

```csharp
Mouse.current.position.ReadValue ( );
```

---