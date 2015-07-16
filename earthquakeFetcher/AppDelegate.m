//
//  AppDelegate.m
//  earthquakeFetcher
//
//  Created by Liam Treacy on 09/07/2015.
//  Copyright (c) 2015 Liam Treacy. All rights reserved.
//

// ::TODO::
// Put the time from the server about when the earthquake actually happened!



#import "AppDelegate.h"
@import WebKit;

NSString * const GEOJSONFeed = @"http://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/all_hour.geojson";

@interface AppDelegate ()

@property (weak) IBOutlet NSTextField *timeLabel;
@property (weak) IBOutlet NSTextField *locationLabel;
@property (weak) IBOutlet NSTextField *magLabel;
@property (weak) IBOutlet WebView *browser;
@property (weak) IBOutlet NSWindow *window;

@property double longitude;
@property double latitude;
@property NSString *html;

@end

@implementation AppDelegate

- (void)applicationDidFinishLaunching:(NSNotification *)aNotification {
}

- (void)applicationWillTerminate:(NSNotification *)aNotification {
}

- (IBAction)updateButtonPressed:(id)sender {
    [self fetchRecentEarthquakes];
}

- (void)updateBrowser {
    WebFrame *mainFrame;
    mainFrame = [_browser mainFrame];
    [mainFrame loadHTMLString:_html baseURL:nil];
}

- (NSMutableString*)loadHtmlFile {
    NSMutableString *result;
    
    NSString *path = [[NSBundle mainBundle] pathForResource:@"placeholder" ofType:@"txt"];
    result = [NSString stringWithContentsOfFile:path encoding:NSUTF8StringEncoding error:nil];
    
    return result;
}

- (NSString*)replaceTokens:(NSMutableString*)str {
    
    // ::TODO:: Clean up this mess...
    NSMutableString *result = [NSMutableString stringWithFormat:str];
    NSMutableString *latt = [NSMutableString stringWithFormat:@"$$1"];
    NSString *tmp = [NSString stringWithFormat:@"%lf", self.latitude];
    NSMutableString *actualLatt = [NSMutableString stringWithFormat:tmp];
    NSMutableString *longg = [NSMutableString stringWithFormat:@"$$2"];
    NSString *tmp2 = [NSString stringWithFormat:@"%lf", self.longitude];
    NSMutableString *actualLongh = [NSMutableString stringWithFormat:tmp2];
    NSRange range = NSMakeRange(0, [result length]);
    
    [result replaceOccurrencesOfString:latt withString:actualLatt options:NSCaseInsensitiveSearch range:range];
    [result replaceOccurrencesOfString:longg withString:actualLongh options:NSCaseInsensitiveSearch range:range];
    
    return result;
}

- (void) updateHtmlString {
    NSMutableString *fileString = [self loadHtmlFile];
    self.html = [self replaceTokens:fileString];
}

- (void) updateMap {
    WebFrame *mainFrame;
    NSURL *url = [[NSBundle mainBundle] URLForResource:@"demo" withExtension:@"html"];
    mainFrame = [_browser mainFrame];
    [mainFrame loadRequest:[NSURLRequest requestWithURL:url]];
}

- (void)fetchRecentEarthquakes {
    
    NSURL *feedURL = [NSURL URLWithString:GEOJSONFeed];
    NSURLRequest *request = [NSURLRequest requestWithURL:feedURL
                                             cachePolicy:NSURLRequestReloadIgnoringLocalAndRemoteCacheData
                                         timeoutInterval:10.0];
    
    NSURLSession *session = [NSURLSession sharedSession];
    NSURLSessionTask *task = [session dataTaskWithRequest:request
                                        completionHandler:^(NSData *data,
                                                            NSURLResponse *response,
                                                            NSError *error) {
                                            if (data) {
                                                NSError *jsonError = nil;
                                                NSDictionary *topLevelDict =
                                                [NSJSONSerialization JSONObjectWithData:data
                                                                                options:0
                                                                                  error:&jsonError];
                                                
                                                if (topLevelDict) {
                                                    
                                                    NSArray *features = topLevelDict[@"features"];
                                                    NSDictionary *firstFeature = features.firstObject;
                                                    NSDictionary *properties = firstFeature[@"properties"];
                                                    NSNumber *magnitude = properties[@"mag"];
                                                    NSString *location = properties[@"place"];
                                                    NSDictionary *geo = firstFeature[@"geometry"];
                                                    NSArray *arr = geo[@"coordinates"];
                                                    
                                                    self.longitude = [arr[0] doubleValue];
                                                    self.latitude = [arr[1] doubleValue];
                                                    
                                                    NSOperationQueue *mainQ = [NSOperationQueue mainQueue];
                                                    [mainQ addOperationWithBlock:^{
                                                        // TODO:: update the time to use received time of quake
                                                        self.timeLabel.stringValue = [NSDate date].description;
                                                        self.magLabel.stringValue = magnitude.stringValue;
                                                        self.locationLabel.stringValue = location;
                                                        
                                                        [self updateHtmlString];
                                                        [self updateBrowser];
                                                    }];
                                                } else {
                                                    NSLog(@"ERROR: Parsing JSON: %@", jsonError);
                                                }
                                            } else {
                                                NSLog(@"ERROR: %@", error);
                                            }
                                        }];
    [task resume];
}

@end
